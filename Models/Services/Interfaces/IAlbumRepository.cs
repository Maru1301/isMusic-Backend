using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using static api.iSMusic.Controllers.MembersController;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IAlbumRepository
	{
		IEnumerable<AlbumIndexDTO> GetRecommended();

		IEnumerable<AlbumIndexDTO> GetAlbumsByGenreId(int genreId, int skipRows, int takeRows);

		AlbumIndexDTO? GetAlbumById(int albumId);

		Album? GetAlbumByIdForCheck(int albumId);

		IEnumerable<AlbumIndexDTO> GetAlbumsByName(string name, int skipRows, int takeRows);

		IEnumerable<AlbumIndexDTO> GetLikedAlbums(int memberId, LikedQueryBody body);
	}
}
