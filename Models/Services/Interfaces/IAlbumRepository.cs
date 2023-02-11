using api.iSMusic.Models.DTOs.MusicDTOs;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IAlbumRepository
	{
		IEnumerable<AlbumIndexDTO> GetRecommended();

		IEnumerable<AlbumIndexDTO> GetAlbumsByGenreId(int genreId, int skipRows, int takeRows);

		AlbumIndexDTO? GetAlbumById(int albumId);

		IEnumerable<AlbumIndexDTO> GetAlbumsByName(string name, int skipRows, int takeRows);
	}
}
