using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IArtistRepository
	{
		ArtistIndexDTO? GetArtistById(int artistId);

		Artist? GetArtistByIdForCheck(int artistId);

		IEnumerable<ArtistIndexDTO> GetArtistsByName(string artistName, int skipRows, int takeRows);
	}
}
