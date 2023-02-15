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
			return _repository.GetCreatorsByName(creatorName, rowNumber);
		}
	}
}
