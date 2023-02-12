using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.PlaylistVMs;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IPlaylistRepository
	{
		IEnumerable<PlaylistIndexDTO> GetRecommended();

		IEnumerable<PlaylistIndexDTO> GetMemberPlaylists(int memberId, int rowNumber);

		IEnumerable<PlaylistIndexDTO> GetMemberPlaylistsByName(int memberId, string name, int rowNumber);

		IEnumerable<PlaylistIndexDTO> GetLikedPlaylists(int memberId);

		Task<int> GetNumOfPlaylistsByMemberIdAsync(int memberId);

		Task CreatePlaylistAsync(PlaylistCreateVM newPlaylist);

		//check
		Task<int> GetPlaylistIdByMemberIdAsync(int memberId);

		PlaylistDetailDTO GetPlaylistById(int playlistId);

		Playlist? GetPlaylistByIdForCheck(int playlistId);

		IEnumerable<PlaylistIndexDTO> GetPlaylistsByName(string name, int skipRows, int takeRows);

		void AddSongToPlaylist(int playlistId, int songId, int lastOrder);
	}
}
