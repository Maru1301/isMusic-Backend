using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IQueueRepository
	{
		QueueIndexDTO? GetQueueById(int queueId);

		Queue? GetQueueByIdForCheck(int queueId);
		
		Task<QueueIndexDTO?> GetQueueByMemberIdAsync(int memberId);

		void AddSongIntoQueue(int queueId, int songId);

		void AddPlaylistIntoQueue(int queueId, int playlistId);

		void UpdateQueueBySong(int queueId, int Id);

		void UpdateQueueBySongs(int queueId, List<int> SongIds, string fromWhere, int contentId);

		void UpdateByDisplayOredr(int queueId, int displayOrder);

		void ChangeShuffle(int queueId);
	}
}
