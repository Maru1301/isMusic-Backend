using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.Services.Interfaces
{
	public interface ISongRepository
	{
		IEnumerable<SongIndexDTO> GetPopularSongs();

		List<int> GetLikedSongIdsByMemberId(int memberId);

		List<SongInfoDTO> GetSongsByPlaylistId(int playlistId);

		IEnumerable<SongGenreInfo> GetSongGenres();

		IEnumerable<SongIndexDTO> SearchBySongName(string songName);
	}
}
