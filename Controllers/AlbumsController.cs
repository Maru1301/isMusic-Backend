using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.AlbumVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AlbumsController : ControllerBase
	{
		private readonly IAlbumRepository _repository;

		private readonly AlbumService _service;

		public AlbumsController(IAlbumRepository repo)
		{
			_repository = repo;
			_service = new AlbumService(_repository);
		}

		[HttpGet]
		[Route("Recommended")]
		public IActionResult GetRecommended()
		{
			var recommendedAlbums = _service.GetRecommended();

			if (!recommendedAlbums.Any())
			{
				return NoContent();
			}

			return Ok(recommendedAlbums.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("SongGenres/{genreId}")]
		public IActionResult GetAlbumsByGenreId(int genreId, [FromQuery]int rowNumber = 2)
		{
			var dtos = _service.GetAlbumsByGenreId(genreId, rowNumber);

			return Ok(dtos.Select(dtos => dtos.ToIndexVM()));
		}

		[HttpGet]
		[Route("{albumName}")]
		public IActionResult GetAlbumByName(string albumName, [FromQuery]int rowNumber = 2)
		{
			var dtos = _service.GetAlbumsByName(albumName, rowNumber);

			return Ok(dtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{albumId}")]
		public IActionResult GetAlbumById(int albumId)
		{
			var dto = _service.GetAlbumById(albumId);

			if(dto == null) return NotFound("專輯不存在");

			return Ok(dto.ToDetailVM());
		}

		
	}
}
