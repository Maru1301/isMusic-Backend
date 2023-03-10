using api.iSMusic.Models;
using api.iSMusic.Models.DTOs.MusicDTOs;
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

		public AlbumsController(IAlbumRepository repo, ISongRepository songRepository)
		{
			_repository = repo;
			_service = new AlbumService(_repository, songRepository);
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
			var result = _service.GetAlbumsByGenreId(genreId, rowNumber);

			if(!result.Success)
			{
				return BadRequest(result.Message);
			}else if (!result.Dtos.Any())
			{
				return NoContent();
			}

			return Ok(result.Dtos.Select(dtos => dtos.ToIndexVM()));
		}

		[HttpGet]
		[Route("Search/{albumName}")]
		public IActionResult GetAlbumByName(string albumName, [FromQuery]int rowNumber = 2)
		{
            var result = _service.GetAlbumsByName(albumName, rowNumber);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}else if(!result.Dtos.Any())
			{
				return NoContent();
			}

			return Ok(result.Dtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{albumId}")]
		public IActionResult GetAlbumById(int albumId)
		{
            int memberId = this.GetMemberId();

            var dto = _service.GetAlbumById(albumId, memberId);

			if(dto == null) return NotFound("專輯不存在");

			return Ok(dto.ToDetailVM());
		}
		[HttpGet]
		[Route("AlbumTypes")]
		public IActionResult GetAlbumTypes()
		{
			var albumtypes = _repository.GetAlbumTypes();

			return Ok(albumtypes);
		}
	}
}
