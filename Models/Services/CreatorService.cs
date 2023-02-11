using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.Identity.Client;

namespace api.iSMusic.Models.Services
{
    public class CreatorService
	{
		private readonly ICreatorRepository _repository;

		public CreatorService(ICreatorRepository repository)
		{
			_repository= repository;
		}

		public IEnumerable<CreatorIndexDTO> GetCreatorsByName(string creatorName, int rowNumber)
		{
			int skip = (rowNumber - 1) * 5;
			int take = 5;
			if(rowNumber == 2)
			{
				skip = 0;
				take = 10;
			}

			return _repository.GetCreatorsByName(creatorName, skip, take);
		}
	}
}
