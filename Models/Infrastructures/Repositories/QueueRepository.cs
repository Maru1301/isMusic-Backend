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

        public Queue? GetQueueByIdForCheck(int queueId)
		{
			return _db.Queues.Find(queueId);
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
				.Select(queue => new QueueIndexDTO
				{
					Id= queue.Id,
                    CurrentSongOrder = queue.CurrentSongOrder,
					CurrentSongTime= queue.CurrentSongTime,
					IsShuffle= queue.IsShuffle,
					IsRepeat= queue.IsRepeat,
					MemberId= memberId,
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
                .Skip(queue.CurrentSongOrder - 1 ?? 0)
                .Take(this.takeLimit)
                .Join(_db.Songs, qs => qs.SongId, s => s.Id, (qs, s) => new SongInfoDTO
                {
                    Id = qs.SongId,
                    SongName = s.SongName,
                    SongCoverPath = s.SongCoverPath,
                    SongPath = s.SongPath,
                    AlbumId = s.AlbumId,
                    AlbumName = s.Album != null ?
                                s.Album.AlbumName :
                                string.Empty,
                    Duration = s.Duration,
                    Status = s.Status,
                    FromList = qs.FromPlaylist,
                    IsExplicit = s.IsExplicit,
                    Released = s.Released,
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
                        SongCoverPath = s.SongCoverPath,
                        SongPath = s.SongPath,
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

		public void UpdateQueueBySongs(int queueId, List<int> songIds, string fromWhere, int contentId)
		{
			//delete old data
			ClearQueueSongs(queueId);

			//check the length of songIds
			songIds = (songIds.Count > storeLimit ? songIds.Take(this.storeLimit) : songIds).ToList();

			//check the queue setting
			var queue = _db.Queues.Single(queue => queue.Id == queueId);
			var isShuffle = queue.IsShuffle;
			var isRepeat = queue.IsRepeat;

			switch (fromWhere)
			{
				case "Artist":
					queue.ArtistId = contentId;
					break;
				case "Album":
					queue.AlbumId = contentId;
					break;
				case "Playlist":
					queue.PlaylistId = contentId;
					break;
			}

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
						QueueId = queueId,
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
						QueueId = queueId,
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

		public void UpdateQueueBySong(int queueId, int songId)
		{
			ClearQueueSongs(queueId);

			var queue = _db.Queues.Single(queue => queue.Id == queueId);
			queue.AlbumId = null;
			queue.ArtistId = null;
			queue.PlaylistId = null;

			var queueSong = new QueueSong()
			{
				QueueId = queueId,
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

		public void UpdateByDisplayOredr(int queueId, int displayOrder)
		{
			var queue = _db.Queues.Single(queue => queue.Id == queueId);

			queue.CurrentSongOrder = displayOrder;

			_db.SaveChanges();
		}

		public void AddSongIntoQueue(int queueId, int songId)
		{
			var qss = _db.QueueSongs
				.Where(qs => qs.QueueId == queueId);

			var lastDisplayOrder = (qss != null) ?
				qss.Max(qs => qs.DisplayOrder) :
				0;

			var newQueueSongData = new QueueSong
			{
				QueueId = queueId,
				SongId = songId,
				FromPlaylist = false,
				DisplayOrder = lastDisplayOrder + 1
			};

			_db.QueueSongs.Add(newQueueSongData);
			_db.SaveChanges();
		}

		public void AddPlaylistIntoQueue(int queueId, int playlistId)
		{
			var qss = _db.QueueSongs
				.Where(qs => qs.QueueId == queueId);

			var displayOrder = (qss!=null) ? 
				qss.Max(qs => qs.DisplayOrder) : 
				0;

			var songIds = _db.PlaylistSongMetadata
				.Where(metadata => metadata.PlayListId == playlistId)
				.Select(metadata => metadata.SongId)
				.ToList();

			var newQueueSongData = songIds.Select((songId, index) => new QueueSong
			{
				QueueId = queueId,
				SongId = songId,
				FromPlaylist = false,
				DisplayOrder = displayOrder + index + 1,
			});

			_db.QueueSongs.AddRange(newQueueSongData);
			_db.SaveChanges();
		}

		public void AddAlbumIntoQueue(int queueId, int albumId)
		{
			var qss = _db.QueueSongs
				.Where(qs => qs.QueueId == queueId);

			var displayOrder = (qss != null) ?
				qss.Max(qs => qs.DisplayOrder) :
				0;

			var songIds = _db.Songs
				.Where(song => song.AlbumId == albumId)
				.Select(song => song.Id)
				.ToList();

			var newQueueSongData = songIds.Select((songId, index) => new QueueSong
			{
				QueueId = queueId,
				SongId = songId,
				FromPlaylist = false,
				DisplayOrder = displayOrder + index + 1,
			});

			_db.QueueSongs.AddRange(newQueueSongData);
			_db.SaveChanges();
		}

		public void ChangeShuffle(int queueId)
		{
			var queue = _db.Queues.Single(queue => queue.Id == queueId);

			queue.IsShuffle = !queue.IsShuffle;

			if (queue.IsShuffle)
			{
				var queueSongs = _db.QueueSongs.Where(qs => qs.QueueId == queueId).ToList();
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

		public void ChangeRepeat(int queueId, string mode)
		{
			var queue = _db.Queues.Single(queue => queue.Id == queueId);

			queue.IsRepeat = mode switch
			{
				"Loop" => true,
				"SingleLoop" => false,
				_ => null,
			};

			_db.SaveChanges();
		}

        public SongInfoDTO? NextSong(int queueId)
        {
            var queue = _db.Queues
                .Include(q => q.QueueSongs)
                .Single(q => q.Id == queueId);

            if (queue.CurrentSongOrder == null)
            {
                throw new InvalidOperationException("佇列為空");
            }

            var queueSongs = queue.QueueSongs;
            int numOfSongs = queueSongs.Count;
			int currentOrder = queue.CurrentSongOrder.Value;

            queue.CurrentSongOrder = queue.IsRepeat switch
            {
                false => currentOrder,
                null => (currentOrder == numOfSongs) ? 0 : currentOrder + 1,
                true => (currentOrder == numOfSongs) ? 1 : currentOrder + 1
            };
            queue.CurrentSongTime = 0;

			var currentSong = queue.QueueSongs.Single(qs => (queue.IsShuffle)?
												qs.ShuffleOrder == currentOrder:
												qs.DisplayOrder == currentOrder);
			if(queue.IsRepeat != false && currentSong.FromPlaylist == false)
			{
				_db.QueueSongs.Remove(currentSong);
			}

            _db.SaveChanges();

            if (queue.IsRepeat == true && numOfSongs - currentOrder < takeLimit)
			{
				int takeOrder = takeLimit - (numOfSongs - currentOrder);

				if(queueSongs.Count >= takeOrder)
				{
					return _db.Songs.Single(song => song.Id == queueSongs.Single(qs => qs.DisplayOrder == takeOrder).SongId).ToInfoDTO();
                }
            }
			
			if(numOfSongs - currentOrder >= takeLimit)
			{
				return _db.Songs.Single(song => song.Id == queueSongs.Single(qs => qs.DisplayOrder == currentOrder + takeLimit).SongId).ToInfoDTO();
            }

            return null;
		}

        public SongInfoDTO? PreviousSong(int queueId)
        {
            var queue = _db.Queues
                .Include(q => q.QueueSongs)
                .Single(q => q.Id == queueId);

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
