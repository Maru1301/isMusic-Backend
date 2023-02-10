using api.iSMusic.Models.DTOs;

namespace api.iSMusic.Models.Services.Interfaces
{
	public interface IAlbumRepository
	{
		IEnumerable<AlbumIndexDTO> GetRecommended();

		IEnumerable<AlbumIndexDTO> GetAlbumsByGenreId(int genreId);
	}
}
