using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class QueueRepository: IRepository, IQueueRepository
	{
		private readonly AppDbContext _db;

		private readonly int queueLimit = 50;

		public QueueRepository(AppDbContext db)
		{
			_db = db;
		}

		public QueueIndexDTO? GetQueueById(int queueId)
		{
			return _db.Queues
				.Include(q => q.QueueSongs)
				.Select(queue => new QueueIndexDTO
				{
					Id = queueId,
					CurrentSongId = queue.CurrentSongId,
					CurrentSongTime = queue.CurrentSongTime,
					IsRepeat = queue.IsRepeat,
					IsShuffle= queue.IsShuffle,
					MemberId= queue.MemberId,
					SongInfos = queue.QueueSongs.OrderBy(qs => qs.DisplayOrder).Select(qs => qs.Song.ToInfoDTO()),
					AlbumId = queue.AlbumId,
					ArtistId= queue.ArtistId,
					PlaylistId= queue.PlaylistId,
				})
				.SingleOrDefault(queue => queue.Id == queueId);
		}

		public async Task<QueueIndexDTO?> GetQueueByMemberIdAsync(int memberId)
		{
			return await _db.Queues
				.Include(q => q.QueueSongs)
				.Select(queue => new QueueIndexDTO
				{
					Id= queue.Id,
					CurrentSongId= queue.CurrentSongId,
					CurrentSongTime= queue.CurrentSongTime,
					IsShuffle= queue.IsShuffle,
					IsRepeat= queue.IsRepeat,
					MemberId= memberId,
					SongInfos = queue.QueueSongs.OrderBy(qs => qs.DisplayOrder).Select(qs => qs.Song.ToInfoDTO()),
					AlbumId = queue.AlbumId,
					ArtistId = queue.ArtistId,
					PlaylistId = queue.PlaylistId,
				})
				.SingleOrDefaultAsync(q => q.MemberId == memberId);
		}

		public void UpdateQueueBySongs(int queueId, List<int> songIds, string fromWhere, int contentId)
		{
			//delete old data
			ClearQueueSongs(queueId);

			//check the length of songIds
			songIds = (songIds.Count > queueLimit ? songIds.Take(this.queueLimit) : songIds).ToList();

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
			var rand = new Random();
			List<QueueSong> queueSongs = new(NumOfSongs);
			int displayOrder = 1;

			foreach (int songId in songIds)
			{
				int order = isShuffle ? rand.Next(NumOfSongs) + 1 : displayOrder++;
				queueSongs.Add(new QueueSong
				{
					QueueId = queueId,
					SongId = songId,
					DisplayOrder = order,
				});
			}

			if (isRepeat.HasValue && isRepeat.Value)
			{
				queueSongs.Add(new QueueSong
				{
					QueueId = queueId,
					SongId = queueSongs.First(x => x.DisplayOrder == 1).SongId,
					DisplayOrder = NumOfSongs + 1,
					FromPlaylist = true,
				});
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

		public void UpdateByQueueSong(int queueId, int songId)
		{
			var queueSongs = _db.QueueSongs
				.Where(qs => qs.QueueId== queueId)
				.OrderBy(qs => qs.DisplayOrder)
				.ToList();

			int newDisplayOrder = 1;
			int currentSongIndex = queueSongs.FindIndex(qs => qs.SongId == songId);

			for (int i = currentSongIndex - 1; i < queueSongs.Count; ++i)
			{
				queueSongs[i].DisplayOrder = newDisplayOrder++;
			}

			for(int i = 0; i < currentSongIndex; ++i)
			{
				queueSongs[i].DisplayOrder = newDisplayOrder++;
			}

			_db.SaveChanges();
		}
	}
}
