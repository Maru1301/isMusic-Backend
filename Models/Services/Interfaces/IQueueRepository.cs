using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IQueueRepository
	{
		void CreateQueue(int memeberId);

		QueueIndexDTO? GetQueueById(int queueId);

        Queue? GetQueueByMemberIdForCheck(int queueId);
		
		Task<QueueIndexDTO?> GetQueueByMemberIdAsync(int memberId);

		void AddSongIntoQueue(int memberId, int songId);

		void AddPlaylistIntoQueue(int memberId, int playlistId);

		void AddAlbumIntoQueue(int memberId, int albumId);

		void UpdateQueueBySong(int memberId, int Id);

		void UpdateQueueBySongs(int memberId, List<int> SongIds, string fromWhere, int contentId);

		void UpdateByDisplayOredr(int queueId, int displayOrder);

        (int? TakeOrder, int NextSongId) NextSong(int memberId);

        int PreviousSong(int memberId);

		void ChangeShuffle(int memberId);

		void ChangeRepeat(int queueId);

        void SavePlayTime(int memberId, int time);
    }
}
