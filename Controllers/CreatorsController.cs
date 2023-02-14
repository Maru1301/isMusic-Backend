using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;
using api.iSMusic.Models.Infrastructures.Extensions;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CreatorsController : ControllerBase
	{
		private readonly ICreatorRepository _repository;

		private readonly CreatorService _service;

		public CreatorsController(ICreatorRepository repository)
		{
			_repository = repository;
			_service = new CreatorService(_repository);
		}

		[HttpGet]
		[Route("{creatorName}")]
		public ActionResult<IEnumerable<CreatorIndexVM>> GetCreatorsByName(string creatorName, [FromQuery]int rowNumber = 2)
		{
			var dtos = _service.GetCreatorsByName(creatorName, rowNumber);

			return Ok(dtos.Select(dto => dto.ToIndexVM()));
		}
	}
}
