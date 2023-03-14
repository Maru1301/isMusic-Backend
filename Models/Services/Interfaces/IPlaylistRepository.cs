using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using static api.iSMusic.Controllers.MembersController;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IPlaylistRepository
	{
		IEnumerable<PlaylistIndexDTO> GetRecommended();

		IEnumerable<PlaylistIndexDTO> GetMemberPlaylists(int memberId, InputQuery query);

		IEnumerable<PlaylistIndexDTO> GetMemberPlaylistsByName(int memberId, string name, int rowNumber);

		IEnumerable<PlaylistIndexDTO> GetLikedPlaylists(int memberId);

		IEnumerable<PlaylistIndexDTO> GetIncludedPlaylists(int artistId, string mode, int rowNumber = 1);

		Task<int> GetNumOfPlaylistsByMemberIdAsync(int memberId);

		Task CreatePlaylistAsync(PlaylistCreateVM newPlaylist);

		Task<int> GetPlaylistIdByMemberIdAsync(int memberId);

		PlaylistDetailDTO GetPlaylistById(int playlistId);

		Playlist? GetPlaylistByIdForCheck(int playlistId);

		IEnumerable<PlaylistIndexDTO> GetPlaylistsByName(string name, int skipRows, int takeRows);

		void AddSongToPlaylist(int playlistId, int songId, int lastOrder);

		void AddSongsToPlaylist(int playlistId, List<int> selectedSongs, int order);

		void UpdatePlaylistDetail(int playlistId, PlaylistEditDTO dto);

		void ChangePrivacySetting(int playlistId);

		void DeletePlaylist(int playlistId);

		void DeleteSongfromPlaylist(int playlistId, int displayOrder);
	}
}
