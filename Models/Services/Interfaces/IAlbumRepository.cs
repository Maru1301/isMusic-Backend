using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using static api.iSMusic.Controllers.MembersController;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IAlbumRepository
	{
		IEnumerable<AlbumIndexDTO> GetRecommended();

		IEnumerable<AlbumIndexDTO> GetAlbumsByGenreId(int genreId, int rowNumber);

		IEnumerable<AlbumIndexDTO>  GetPopularAlbums(int artistId, string mode, int rowNumber = 1);

		AlbumDetailDTO? GetAlbumById(int albumId);

		Album? GetAlbumByIdForCheck(int albumId);

		IEnumerable<AlbumIndexDTO> GetAlbumsByName(string name, int rowNumber);

		IEnumerable<AlbumIndexDTO> GetLikedAlbums(int memberId, LikedQuery query);

		IEnumerable<AlbumIndexDTO> GetAlbumsByContentId(int content, string mode, int rowNumber);
		IEnumerable<AlbumTypeDTO> GetAlbumTypes();

        LikedAlbum? CheckIsLiked(int albumId, int memberId);
    }
}
