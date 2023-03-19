using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.SongVMs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class QueueRepository: IRepository, IQueueRepository
	{
		private readonly AppDbContext _db;

		private readonly int storeLimit = 1000;

		private readonly int takeLimit = 50;

		public QueueRepository(AppDbContext db)
		{
			_db = db;
		}

		public void CreateQueue(int memberId)
		{
            var queue = new Queue()
			{
				MemberId = memberId,
				CurrentSongOrder = 0,
				CurrentSongTime = null,
				IsShuffle = false,
				IsRepeat = null,
				AlbumId= null,
				ArtistId= null,
				PlaylistId= null,
			};
			_db.Queues.Add(queue);
			_db.SaveChanges();
		}

        public Queue? GetQueueByMemberIdForCheck(int memberId)
		{
			return _db.Queues.SingleOrDefault(queue => queue.MemberId == memberId);
		}

		public QueueIndexDTO? GetQueueById(int queueId)
		{
            var queue = _db.Queues
				.Include(q => q.QueueSongs)
				.Select(queue => new QueueIndexDTO
				{
					Id = queueId,
                    CurrentSongOrder = queue.CurrentSongOrder,
					CurrentSongTime = queue.CurrentSongTime,
					IsRepeat = queue.IsRepeat,
					IsShuffle= queue.IsShuffle,
					MemberId= queue.MemberId,
                    AlbumId = queue.AlbumId,
					ArtistId= queue.ArtistId,
					PlaylistId= queue.PlaylistId,
				})
				.Single(queue => queue.Id == queueId);

            //check isRepeat
            queue = CheckisRepeat(queue);

			return queue;
		}

        public async Task<QueueIndexDTO?> GetQueueByMemberIdAsync(int memberId)
		{
			var queue = await _db.Queues
				.Include(q => q.QueueSongs)
					.ThenInclude(qss => qss.Song)
					.ThenInclude(song => song.SongArtistMetadata)
				.Include(queue => queue.QueueSongs)
					.ThenInclude(qss => qss.Song)
					.ThenInclude(song => song.SongCreatorMetadata)
				.Select(queue => new QueueIndexDTO
				{
					Id = queue.Id,
					CurrentSongOrder = queue.CurrentSongOrder,
					CurrentSongTime = queue.CurrentSongTime,
					IsShuffle = queue.IsShuffle,
					IsRepeat = queue.IsRepeat,
                    InList = queue.InList,
					MemberId = queue.MemberId,
					AlbumId = queue.AlbumId,
					ArtistId = queue.ArtistId,
					PlaylistId = queue.PlaylistId,
					SongInfos = new(),
				})
				.SingleAsync(q => q.MemberId == memberId);

            //check isRepeat
            queue = CheckisRepeat(queue);

			return queue;
		}

		private QueueIndexDTO? CheckisRepeat(QueueIndexDTO queue)
		{
			var queueSongs = _db.QueueSongs
				.Include(qs => qs.Song)
					.ThenInclude(song => song.SongArtistMetadata)
					.ThenInclude(metadata => metadata.Artist)
				.Include(qs => qs.Song)
					.ThenInclude(song => song.SongCreatorMetadata)
					.ThenInclude(metadata => metadata.Creator)
				.Where(qs => qs.QueueId == queue.Id);

			var fromList = queueSongs
				.Where(qs => qs.FromPlaylist)
				.OrderBy(qs => queue.IsShuffle ? qs.ShuffleOrder : qs.DisplayOrder);
			var notFromList = queueSongs
				.Where(qs => !qs.FromPlaylist )
				.OrderBy(qs => qs.DisplayOrder)
				.Select(qs => qs.Song)
				.Select(song => song.ToInfoDTONotFromList());

			var takeFromList = fromList
				.Where(list => (queue.IsShuffle)
					? list.ShuffleOrder >= queue.CurrentSongOrder
					: list.DisplayOrder >= queue.CurrentSongOrder)
                .Select(qs => qs.Song).Select(song => song.ToInfoDTO());

			if (queue.InList)
			{
                queue.SongInfos.Add(takeFromList.First());
            }
            queue.SongInfos.AddRange(notFromList);
			if (queue.InList)
			{
                queue.SongInfos.AddRange(takeFromList.Skip(1));
			}
			else
			{
				queue.SongInfos.AddRange(takeFromList);
			}

            if (queue.IsRepeat == true && queue.SongInfos.Count < this.takeLimit)
            {
                // If the queue is set to repeat and the number of songs retrieved is less than the takeLimit,
                // retrieve songs from the beginning of the queue until the takeLimit is reached.
                var remainingSongCount = this.takeLimit - queue.SongInfos.Count;

				queue.SongInfos.AddRange(fromList.Select(qs => qs.Song).Select(song => song.ToInfoDTO()).Take(remainingSongCount));
            }

			return queue;
		}

		public void UpdateQueueBySongs(int memberId, List<int> songIds, string fromWhere, int contentId)
		{
			//check the queue setting
			var queue = _db.Queues.Single(queue => queue.MemberId == memberId);
			var isShuffle = queue.IsShuffle;
			var isRepeat = queue.IsRepeat;

            queue.ArtistId = fromWhere == "Artist" ? contentId : (int?)null;
            queue.AlbumId = fromWhere == "Album" ? contentId : (int?)null;
            queue.PlaylistId = fromWhere == "Playlist" ? contentId : (int?)null;

            //delete old data
            ClearQueueSongs(queue.Id);

            //update new songs
            int NumOfSongs = songIds.Count;
			List<QueueSong> queueSongs = new();
            int totalNumOfSongs = NumOfSongs;
            var rand = new Random();

            var shuffleOrders = new HashSet<int>();
            int shuffleOrder = 2;
            int displayOrder = 1;

            foreach (int songId in songIds)
			{
				do
				{
                    shuffleOrder = rand.Next(totalNumOfSongs) + 1;
				} while (!shuffleOrders.Add(shuffleOrder));

				queueSongs.Add(new QueueSong
				{
					QueueId = queue.Id,
					SongId = songId,
					ShuffleOrder = shuffleOrder,
                    DisplayOrder = displayOrder++,
                    FromPlaylist = true,
				});
			}

			queue.CurrentSongOrder = 1;
			queue.CurrentSongTime = 0;

			_db.QueueSongs.AddRange(queueSongs);
			_db.SaveChanges();
		}

		public void UpdateQueueBySong(int memberId, int songId)
		{
            var queue = _db.Queues.Single(queue => queue.MemberId == memberId);

            ClearQueueSongs(queue.Id);

			queue.AlbumId = null;
			queue.ArtistId = null;
			queue.PlaylistId = null;

			var queueSong = new QueueSong()
			{
				QueueId = queue.Id,
				SongId = songId,
				DisplayOrder = 1,
				FromPlaylist = false,
			};

			queue.CurrentSongOrder = 1;
			queue.CurrentSongTime = 0;

			_db.QueueSongs.Add(queueSong);
			_db.SaveChanges();
		}

		private void ClearQueueSongs(int queueId)
		{
			_db.QueueSongs.RemoveRange(_db.QueueSongs.Where(metadata => metadata.QueueId == queueId && metadata.FromPlaylist == true));
		}

		public void UpdateByDisplayOredr(int memberId, int displayOrder)
		{
			var queue = _db.Queues.Include(queue => queue.QueueSongs).Single(queue => queue.MemberId == memberId);

			var toBeRemoved = queue.QueueSongs
						.Where(qs => qs.FromPlaylist)
						.Where(qs => (queue.IsShuffle) 
							? qs.ShuffleOrder < displayOrder 
							: qs.DisplayOrder < displayOrder);

			queue.CurrentSongOrder = displayOrder;

            foreach (var queueSong in toBeRemoved)
            {
                _db.QueueSongs.Remove(queueSong);
            }

            _db.SaveChanges();
		}

		public void AddSongIntoQueue(int memberId, int songId)
		{
			var queue = GetQueueByMemberId(memberId);

            var queueSongs = queue.QueueSongs;

            var lastDisplayOrder = queueSongs != null ?
                (queue.IsShuffle ?
                    queueSongs.Max(qs => qs.ShuffleOrder) :
                    queueSongs.Max(qs => qs.DisplayOrder)
                ) :
                0;

            var newQueueSongData = new QueueSong
			{
				QueueId = queue.Id,
				SongId = songId,
				FromPlaylist = false,
				DisplayOrder = lastDisplayOrder + 1
			};

			_db.QueueSongs.Add(newQueueSongData);
			_db.SaveChanges();
		}

		private Queue GetQueueByMemberId(int memberId)
		{
			return _db.Queues
                .Include(queue => queue.QueueSongs)
                .Single(queue => queue.MemberId == memberId);
        }

		public void AddPlaylistIntoQueue(int memberId, int playlistId)
		{
            var queue = GetQueueByMemberId(memberId);

			var queueSongs = queue.QueueSongs.Where(qs => !qs.FromPlaylist);

			var lastDisplayOrder = queueSongs.Count() != 0
				?queueSongs.Max(qs => qs.DisplayOrder)
				: 0;

			var songIds = _db.PlaylistSongMetadata
				.Where(metadata => metadata.PlayListId == playlistId)
				.OrderBy(metadata => metadata.DisplayOrder)
				.Select(metadata => metadata.SongId)
				.ToList();

			var newQueueSongData = songIds.Select((songId, index) => new QueueSong
			{
				QueueId = queue.Id,
				SongId = songId,
				FromPlaylist = false,
				DisplayOrder = lastDisplayOrder + index + 1,
			});

			_db.QueueSongs.AddRange(newQueueSongData);
			_db.SaveChanges();
		}

		public void AddAlbumIntoQueue(int memberId, int albumId)
		{
			var queue = GetQueueByMemberId(memberId);

            var queueSongs = queue.QueueSongs.Where(qs => !qs.FromPlaylist);

            var lastDisplayOrder = queueSongs.Count() != 0
                ? queueSongs.Max(qs => qs.DisplayOrder)
                : 0;

            var songIds = _db.Songs
				.Where(song => song.AlbumId == albumId)
				.OrderBy(song => song.DisplayOrderInAlbum)
				.Select(song => song.Id)
				.ToList();

			var newQueueSongData = songIds.Select((songId, index) => new QueueSong
			{
				QueueId = queue.Id,
				SongId = songId,
				FromPlaylist = false,
				DisplayOrder = lastDisplayOrder + index + 1,
			});

			_db.QueueSongs.AddRange(newQueueSongData);
			_db.SaveChanges();
		}

		public void ChangeShuffle(int memberId)
		{
			var queue = _db.Queues.Single(queue => queue.MemberId == memberId);

			queue.IsShuffle = !queue.IsShuffle;
            var queueSongs = _db.QueueSongs.Where(qs => qs.QueueId == queue.Id && qs.FromPlaylist).ToList();

            if (queue.IsShuffle)
			{
				
				int numOfSongs = queueSongs.Count;
				var rand = new Random();
				var orders = new HashSet<int>();

				if (queue.InList)
				{
					var currentSong = queueSongs.Single(qs => qs.DisplayOrder == queue.CurrentSongOrder);
					currentSong.ShuffleOrder = queue.CurrentSongOrder;
					orders.Add(queue.CurrentSongOrder);
					queueSongs.Remove(currentSong);
				}

				foreach (var queueSong in queueSongs)
				{
					int order;
					do
					{
						order = rand.Next(numOfSongs) + 1;
					} while (!orders.Add(order));

					queueSong.ShuffleOrder = order;
				}
			}
			else
			{
                var currentSong = queueSongs.Single(qs => qs.ShuffleOrder == queue.CurrentSongOrder);
				queue.CurrentSongOrder = currentSong.DisplayOrder;
            }

			_db.SaveChanges();
		}

		public void ChangeRepeat(int memberId)
		{
			var queue = _db.Queues.Single(queue => queue.MemberId == memberId);

			queue.IsRepeat = !queue.IsRepeat;

            _db.SaveChanges();
		}

        public void SavePlayTime(int memberId, int time)
		{
			var queue = _db.Queues.Single(queue => queue.MemberId == memberId);

			queue.CurrentSongTime = time;

			_db.SaveChanges();
		}

        public (int? TakeOrder, int NextSongId) NextSong(int memberId)
        {
            var queue = _db.Queues
                .Include(q => q.QueueSongs)
                .Single(q => q.MemberId == memberId);

            if (queue.CurrentSongOrder == 0) 
				throw new InvalidOperationException("佇列為空");

            var currentSong = FindCurrentSong(queue);

            //if the current song is not from a list
            if (currentSong.FromPlaylist == false)
                _db.QueueSongs.Remove(currentSong);

            _db.SaveChanges();

            queue = _db.Queues
                .Include(q => q.QueueSongs)
                .Single(q => q.MemberId == memberId);

            UpdateCurrentSongOrder(queue);

            if (queue.QueueSongs.Where(qs => !qs.FromPlaylist).Count() == 0)
            {
                queue.InList = true;
            }
            else
            {
                queue.InList = false;
            }

            _db.SaveChanges();

            //find the next song id
            int nextSongId = FindNextSongId(queue);

            var queueSongs = queue.QueueSongs.OrderBy(qs => queue.IsShuffle ? qs.ShuffleOrder : qs.DisplayOrder).ToList();

            int currentIndex = queueSongs.IndexOf(currentSong) + 1;
            int numOfSongs = queueSongs.Count;

            //find the addedQueueSong display order
            if (queue.IsRepeat == true && numOfSongs - currentIndex < takeLimit)
			{
				int takeOrder = takeLimit - (numOfSongs - currentIndex);

				if(numOfSongs >= takeOrder)
				{
                    return (takeOrder, nextSongId);
                }
            }
			
			if(numOfSongs - currentIndex >= takeLimit)
			{
				return (currentIndex + takeLimit, nextSongId);
            }

            return (null, nextSongId);
		}

        private void UpdateCurrentSongOrder(Queue queue)
        {
			if (queue.InList)
			{
                var queueSongs = queue.QueueSongs.Where(qs => qs.FromPlaylist);
                int firstOrder = queueSongs.Min(qs => (queue.IsShuffle)
                                                    ? qs.ShuffleOrder
                                                    : qs.DisplayOrder);
                int lastOrder = queueSongs.Max(qs => (queue.IsShuffle)
                                                    ? qs.ShuffleOrder
                                                    : qs.DisplayOrder);
                int currentOrder = queue.CurrentSongOrder;

                queue.CurrentSongOrder = (queue.IsRepeat == true)
                    ? (currentOrder == lastOrder) ? firstOrder : currentOrder + 1
                    : (currentOrder == lastOrder) ? 0 : currentOrder + 1;

                queue.CurrentSongTime = 0;

                if (queue.CurrentSongOrder == 0)
                {
                    queue.AlbumId = null;
                    queue.ArtistId = null;
                    queue.PlaylistId = null;
                    ClearQueueSongs(queue.Id);
                    _db.SaveChanges();
                    throw new InvalidOperationException("佇列沒有下一首歌");
                }
            }

            _db.SaveChanges();
        }

        private QueueSong FindCurrentSong(Queue queue)
        {
            if (queue.CurrentSongOrder == 0)
            {
                throw new InvalidOperationException("佇列為空");
            }

			QueueSong currentSong;
            if (queue.InList)
			{
                currentSong = queue.QueueSongs
					.Where(qs => qs.FromPlaylist)
                    .Single(qs =>
                    (queue.IsShuffle)
                    ? qs.ShuffleOrder == queue.CurrentSongOrder
                    : qs.DisplayOrder == queue.CurrentSongOrder);
			}
			else
			{
				currentSong = queue.QueueSongs
					.Where(qs => !qs.FromPlaylist)
					.First();
			}

            if (currentSong == null)
            {
                throw new InvalidOperationException("找不到目前播放歌曲");
            }

            return currentSong;
        }

        private int FindNextSongId(Queue queue)
        {
            int nextSongOrder = queue.CurrentSongOrder;
            int nextSongId = queue.QueueSongs.Single(qs => (queue.IsShuffle) 
				? qs.ShuffleOrder == nextSongOrder
				: qs.DisplayOrder == nextSongOrder).SongId;
            return nextSongId;
        }

        public int PreviousSong(int memberId)
        {
            var queue = _db.Queues
                .Include(q => q.QueueSongs)
                .Single(q => q.MemberId == memberId);

            if (queue.CurrentSongOrder == 0)
            {
                throw new InvalidOperationException("佇列為空");
            }

            var queueSongs = queue.QueueSongs.Where(qs => qs.FromPlaylist);
            int firstOrder = queueSongs.Min(qs => (queue.IsShuffle)
                                                ? qs.ShuffleOrder
                                                : qs.DisplayOrder);
            int lastOrder = queueSongs.Max(qs => (queue.IsShuffle)
                                                ? qs.ShuffleOrder
                                                : qs.DisplayOrder);
            int currentOrder = queue.CurrentSongOrder;

            queue.CurrentSongOrder = queue.IsRepeat switch
            {
                false => currentOrder,
                null => (currentOrder == firstOrder) ? 
						firstOrder : 
						currentOrder - 1,
                true => (currentOrder == firstOrder) ? 
						queueSongs.Count() : 
						currentOrder - 1
            };
            queue.CurrentSongTime = 0;

            _db.SaveChanges();

			return queueSongs.Where(qs => queue.IsShuffle ? qs.ShuffleOrder == queue.CurrentSongOrder : qs.DisplayOrder == queue.CurrentSongOrder).Single().SongId;
        }
    }
}
