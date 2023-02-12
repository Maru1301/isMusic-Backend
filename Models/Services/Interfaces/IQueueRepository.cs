using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IQueueRepository
	{
		QueueIndexDTO? GetQueueById(int queueId);

		Task<QueueIndexDTO?> GetQueueByMemberIdAsync(int memberId);

		void UpdateQueueBySong(int queueId, int Id);

		void UpdateQueueBySongs(int queueId, List<int> SongIds, string fromWhere, int contentId);

		void UpdateByQueueSong(int queueId, int songId);
	}
}
