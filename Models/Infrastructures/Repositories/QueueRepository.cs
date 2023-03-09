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
				CurrentSongOrder = null,
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
					Id= queue.Id,
                    CurrentSongOrder = queue.CurrentSongOrder,
					CurrentSongTime= queue.CurrentSongTime,
					IsShuffle= queue.IsShuffle,
					IsRepeat= queue.IsRepeat,
					MemberId= queue.MemberId,
					AlbumId = queue.AlbumId,
					ArtistId = queue.ArtistId,
					PlaylistId = queue.PlaylistId,
				})
				.SingleAsync(q => q.MemberId == memberId);

            //check isRepeat
            queue = CheckisRepeat(queue);

			return queue;
		}

		private QueueIndexDTO? CheckisRepeat(QueueIndexDTO queue)
		{
			queue.SongInfos = _db.QueueSongs
				.Where(qs => qs.QueueId == queue.Id)
				.OrderBy(qs => queue.IsShuffle ? qs.ShuffleOrder : qs.DisplayOrder)
				.Take(this.takeLimit)
				.Join(_db.Songs, qs => qs.SongId, s => s.Id, (qs, s) => new SongInfoDTO
				{
					Id = qs.SongId,
					SongName = s.SongName,
					SongCoverPath = "https://localhost:44373/Uploads/Covers/" + s.SongCoverPath,
					SongPath = "https://localhost:44373/Uploads/Songs/" + s.SongPath,
					AlbumId = s.AlbumId,
					AlbumName = s.Album != null ?
								s.Album.AlbumName :
								string.Empty,
					Duration = s.Duration,
					Status = s.Status,
					FromList = qs.FromPlaylist,
					IsExplicit = s.IsExplicit,
					Released = s.Released,
					Artists = s.SongArtistMetadata.Select(metadata => metadata.Artist).Select(artist => artist.ToInfoVM()).ToList(),
					Creators = s.SongCreatorMetadata.Select(metadata => metadata.Creator).Select(creator => creator.ToInfoVM()).ToList(),
				})
                .ToList();

            if (queue.IsRepeat == true && queue.SongInfos.Count < this.takeLimit)
            {
                // If the queue is set to repeat and the number of songs retrieved is less than the takeLimit,
                // retrieve songs from the beginning of the queue until the takeLimit is reached.
                var remainingSongCount = this.takeLimit - queue.SongInfos.Count;
                var repeatSongs = _db.QueueSongs
                    .Where(qs => qs.QueueId == queue.Id)
                    .OrderBy(qs => queue.IsShuffle ? qs.ShuffleOrder : qs.DisplayOrder)
                    .Take(remainingSongCount)
                    .Join(_db.Songs, qs => qs.SongId, s => s.Id, (qs, s) => new SongInfoDTO
                    {
                        Id = s.Id,
                        SongName = s.SongName,
                        SongCoverPath = "https://localhost:44373/Uploads/Covers/" +s.SongCoverPath,
                        SongPath = "https://localhost:44373/Uploads/Songs/" + s.SongPath,
                        AlbumId = s.AlbumId,
                        AlbumName = s.Album != null ?
                                s.Album.AlbumName :
                                string.Empty,
                        Duration = s.Duration,
                        Status = s.Status,
                        FromList = qs.FromPlaylist,
                        IsExplicit = s.IsExplicit,
                        Released = s.Released,
                    });
                queue.SongInfos.AddRange(repeatSongs);
            }

			return queue;
		}

		public void UpdateQueueBySongs(int memberId, List<int> songIds, string fromWhere, int contentId)
		{
			//check the length of songIds
			songIds = (songIds.Count > storeLimit ? songIds.Take(this.storeLimit) : songIds).ToList();

			//check the queue setting
			var queue = _db.Queues.Single(queue => queue.MemberId == memberId);
			var isShuffle = queue.IsShuffle;
			var isRepeat = queue.IsRepeat;

            //delete old data
            ClearQueueSongs(queue.Id);

            queue.ArtistId = fromWhere == "Artist" ? contentId : (int?)null;
            queue.AlbumId = fromWhere == "Album" ? contentId : (int?)null;
            queue.PlaylistId = fromWhere == "Playlist" ? contentId : (int?)null;

            //update new songs
            int NumOfSongs = songIds.Count;
			List<QueueSong> queueSongs = new(NumOfSongs);

			if (isShuffle)
			{
				int numOfSongs = queueSongs.Count;
				var rand = new Random();
				var orders = new HashSet<int>();

				foreach (int songId in songIds)
				{
					int order;
					do
					{
						order = rand.Next(numOfSongs) + 1;
					} while (!orders.Add(order));

					queueSongs.Add(new QueueSong
					{
						QueueId = queue.Id,
						SongId = songId,
						DisplayOrder = order,
					});
				}
			}
			else
			{
				int displayOrder = 1;

				foreach(int songId in songIds)
				{
					queueSongs.Add(new QueueSong
					{
						QueueId = queue.Id,
						SongId = songId,
						DisplayOrder = displayOrder++,
					});
				}
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
			_db.QueueSongs.RemoveRange(_db.QueueSongs.Where(metadata => metadata.QueueId == queueId));
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

			var queueSongs = queue.QueueSongs;

            var lastDisplayOrder = queueSongs != null ?
                (queue.IsShuffle ? 
					queueSongs.Max(qs => qs.ShuffleOrder): 
					queueSongs.Max(qs => qs.DisplayOrder)
				) : 
				0;

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

			var queueSongs = queue.QueueSongs;

            var lastDisplayOrder = queueSongs != null ?
                (queue.IsShuffle ?
                    queueSongs.Max(qs => qs.ShuffleOrder) :
                    queueSongs.Max(qs => qs.DisplayOrder)
                ) :
                0;

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

			if (queue.IsShuffle)
			{
				var queueSongs = _db.QueueSongs.Where(qs => qs.QueueId == queue.Id).ToList();
				int numOfSongs = queueSongs.Count;
				var rand = new Random();
				var orders = new HashSet<int>();

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

			_db.SaveChanges();
		}

		public void ChangeRepeat(int memberId, string mode)
		{
			var queue = _db.Queues.Single(queue => queue.MemberId == memberId);

			queue.IsRepeat = mode switch
			{
				"Loop" => true,
				"SingleLoop" => false,
				_ => null,
			};

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

			int currentOrder = UpdateCurrentSongOrder(queue);

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

        private int UpdateCurrentSongOrder(Queue queue)
        {
            var queueSongs = queue.QueueSongs;
            int firstOrder = queueSongs.Min(qs => (queue.IsShuffle)
                                                ? qs.ShuffleOrder
                                                : qs.DisplayOrder);
            int lastOrder = queueSongs.Max(qs => (queue.IsShuffle)
                                                ? qs.ShuffleOrder
                                                : qs.DisplayOrder);
            int currentOrder = queue.CurrentSongOrder!.Value;

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

			return currentOrder;
        }

        private QueueSong FindCurrentSong(Queue queue)
        {
            if (queue.CurrentSongOrder == 0)
            {
                throw new InvalidOperationException("佇列為空");
            }

            var currentSong = queue.QueueSongs.SingleOrDefault(qs =>		
					(queue.IsShuffle)
					? qs.ShuffleOrder == queue.CurrentSongOrder
					: qs.DisplayOrder == queue.CurrentSongOrder);

            if (currentSong == null)
            {
                throw new InvalidOperationException("找不到目前播放歌曲");
            }

            return currentSong;
        }

        private int FindNextSongId(Queue queue)
        {
            int nextSongOrder = queue.CurrentSongOrder!.Value;
            int nextSongId = queue.QueueSongs.Single(qs => (queue.IsShuffle) 
								? qs.ShuffleOrder == nextSongOrder
								: qs.DisplayOrder == nextSongOrder).SongId;
            return nextSongId;
        }

        public SongInfoDTO? PreviousSong(int memberId)
        {
            var queue = _db.Queues
                .Include(q => q.QueueSongs)
                .Single(q => q.MemberId == memberId);

            if (queue.CurrentSongOrder == null)
            {
                throw new Exception("佇列為空");
            }

            var queueSongs = queue.QueueSongs;
            int firstOrder = 1;
            int currentOrder = queue.CurrentSongOrder.Value;

            queue.CurrentSongOrder = queue.IsRepeat switch
            {
                false => currentOrder,
                null => (currentOrder == firstOrder) ? 
						firstOrder : 
						currentOrder - 1,
                true => (currentOrder == firstOrder) ? 
						queueSongs.Count : 
						currentOrder - 1
            };
            queue.CurrentSongTime = 0;

            _db.SaveChanges();

			if(queue.IsRepeat == false)
			{
				return null;
			}

            return _db.Songs.Single(song => song.Id == queueSongs.Single(qs => qs.DisplayOrder == queue.CurrentSongOrder).SongId).ToInfoDTO();
        }
    }
}
