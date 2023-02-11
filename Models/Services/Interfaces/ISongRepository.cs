using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface ISongRepository
	{
		IEnumerable<SongIndexDTO> GetPopularSongs(int artistId = 0);

		IEnumerable<int> GetLikedSongIdsByMemberId(int memberId);

		IEnumerable<SongIndexDTO> GetRecentlyPlayed(int memberId);

		IEnumerable<SongInfoDTO> GetSongsByAlbumId(int albumId);

		IEnumerable<SongInfoDTO> GetSongsByPlaylistId(int playlistId);

		IEnumerable<SongGenreInfo> GetSongGenres();

		SongIndexDTO? GetSongById(int id);

		IEnumerable<SongIndexDTO> GetSongsByName(string name, int skipRows, int takeRows);
	}
}
