using api.iSMusic.Models.DTOs.MusicDTOs;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface IArtistRepository
	{
		ArtistIndexDTO? GetArtistById(int id);

		IEnumerable<ArtistIndexDTO> GetArtistsByName(string artistName, int skipRows, int takeRows);
	}
}
