using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface ISongRepository
	{
		IEnumerable<SongIndexDTO> GetPopularSongs(int contentId = 0, string mode = "", int rowNumber = 1);

		IEnumerable<int> GetLikedSongIdsByMemberId(int memberId);

		IEnumerable<SongIndexDTO> GetRecentlyPlayed(int memberId);

		IEnumerable<SongInfoDTO> GetSongsByAlbumId(int albumId);

		IEnumerable<SongInfoDTO> GetSongsByPlaylistId(int playlistId);

		IEnumerable<SongGenreInfo> GetSongGenres();

		SongIndexDTO? GetSongById(int songId);

        SongIndexDTO? GetSongByQueueOrder(int memberId, int takeOrder);

        Song? GetSongByIdForCheck(int songId);

		IEnumerable<SongIndexDTO> GetSongsByName(string name, int rowNumber);

        void CreatePlayRecord(int songId, int memberId);

		void CreateUploadSong(string coverPath, string songPath, CreatorUploadSongDTO creatoruploadsongdto);

	}
}
