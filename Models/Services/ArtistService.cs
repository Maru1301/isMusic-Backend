using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.Services.Interfaces;

namespace api.iSMusic.Models.Services
{
    public class ArtistService
	{
		private readonly IArtistRepository _artistRepository;

		public ArtistService(IArtistRepository artistRepository)
		{
			_artistRepository= artistRepository;
		}

		public IEnumerable<ArtistIndexDTO> GetArtistsByName(string name, int rowNumber)
		{
			int skip = (rowNumber - 1) * 5;
			int take = 5;
			if(rowNumber == 2)
			{
				skip = 0;
				take = 10;
			}

			return _artistRepository.GetArtistsByName(name, skip, take);
		}
	}
}
