using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using static api.iSMusic.Controllers.MembersController;
using static api.iSMusic.Controllers.QueuesController;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IArtistRepository
	{
        IEnumerable<ArtistIndexDTO>  GetRecommended();

        ArtistIndexDTO? GetArtistById(int artistId);

		Artist? GetArtistByIdForCheck(int artistId);

		IEnumerable<ArtistIndexDTO> GetArtistsByName(string artistName, int skipRows, int takeRows);

		IEnumerable<ArtistIndexDTO> GetLikedArtists(int memberId, LikedQuery body);

		ArtistAboutDTO GetArtistAbout(int artistId);
	}
}
