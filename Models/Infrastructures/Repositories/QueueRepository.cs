using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
					CurrentSongId = queue.CurrentSongId,
					CurrentSongTime = queue.CurrentSongTime,
					IsRepeat = queue.IsRepeat,
					IsShuffle= queue.IsShuffle,
					MemberId= queue.MemberId,
					SongInfos = queue.IsShuffle ?
						queue.QueueSongs
						.OrderBy(qs => qs.ShuffleOrder)
						.Take(this.takeLimit)
						.Select(qs => qs.Song.ToInfoDTO())
						.ToList() :
						queue.QueueSongs
						.OrderBy(qs => qs.DisplayOrder)
						.Take(this.takeLimit)
						.Select(qs => qs.Song.ToInfoDTO())
						.ToList(),
					AlbumId = queue.AlbumId,
					ArtistId= queue.ArtistId,
					PlaylistId= queue.PlaylistId,
					QueueSongs = queue.QueueSongs.OrderBy(qs => qs.DisplayOrder).ToList(),
				})
				.Single(queue => queue.Id == queueId);

			//update the song property "FromList"
			var fromLists = queue.QueueSongs.Select(qs => qs.FromPlaylist).ToList();

			for(int i = 0; i < queue.SongInfos.Count; ++i)
			{
				queue.SongInfos[i].FromList = fromLists[i];
			}

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
					CurrentSongId= queue.CurrentSongId,
					CurrentSongTime= queue.CurrentSongTime,
					IsShuffle= queue.IsShuffle,
					IsRepeat= queue.IsRepeat,
					MemberId= memberId,
					SongInfos = queue.IsShuffle ? 
						queue.QueueSongs
						.OrderBy(qs => qs.ShuffleOrder)
						.Take(this.takeLimit)
						.Select(qs => qs.Song.ToInfoDTO())
						.ToList() :
						queue.QueueSongs.OrderBy(qs => qs.DisplayOrder)
						.Take(this.takeLimit)
						.Select(qs => qs.Song.ToInfoDTO())
						.ToList(),
					AlbumId = queue.AlbumId,
					ArtistId = queue.ArtistId,
					PlaylistId = queue.PlaylistId,
				})
				.SingleAsync(q => q.MemberId == memberId);

			//check isRepeat
			queue = CheckisRepeat(queue);

			return queue;
		}

		private QueueIndexDTO? CheckisRepeat(QueueIndexDTO? queue)
		{
			if (queue == null)
			{
				return null;
			}

			if (queue.IsRepeat.HasValue && queue.IsRepeat.Value == true && queue.SongInfos.Count < this.takeLimit)
			{
				queue.SongInfos.Add(queue.SongInfos.First());
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

			queue.CurrentSongId = queueSongs.OrderBy(qs => qs.DisplayOrder).First().SongId;
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

			queue.CurrentSongId = queueSong.SongId;
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
			var isShuffle = _db.Queues.Single(queue => queue.Id == queueId).IsShuffle;

			var queueSongs = _db.QueueSongs
			.Where(qs => qs.QueueId == queueId)
			.OrderBy(qs => isShuffle ? qs.ShuffleOrder : qs.DisplayOrder)
			.ToList();

			int newDisplayOrder = 1;

			foreach (var song in queueSongs)
			{
				int currentIndex = (song.DisplayOrder + displayOrder - 1) % queueSongs.Count;

				var targetSong = queueSongs[currentIndex];
				if (isShuffle)
					targetSong.ShuffleOrder = newDisplayOrder++;
				else
					targetSong.DisplayOrder = newDisplayOrder++;
			}

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
	}
}
