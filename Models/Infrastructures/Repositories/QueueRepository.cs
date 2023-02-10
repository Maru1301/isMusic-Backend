using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
	public class QueueRepository: IRepository, IQueueRepository
	{
		private readonly AppDbContext _db;

		public QueueRepository(AppDbContext db)
		{
			_db = db;
		}

		public async Task<QueueIndexDTO?> GetQueueByMemberIdAsync(int memberId)
		{
			return await _db.Queues
				.Include(q => q.CurrentSong)
				.Include(q => q.QueueSongs)
				.Select(queue => new QueueIndexDTO
				{
					Id= queue.Id,
					CurrentSong= queue.CurrentSong,
					CurrentSongId= queue.CurrentSongId,
					CurrentSongTime= queue.CurrentSongTime,
					IsShuffle= queue.IsShuffle,
					IsRepeat= queue.IsRepeat,
					MemberId= memberId,
					SongIds = queue.QueueSongs.Select(qs => qs.SongId),
				})
				.SingleOrDefaultAsync(q => q.MemberId == memberId);
		}
	}
}
