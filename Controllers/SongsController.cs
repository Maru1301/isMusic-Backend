using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using api.iSMusic.Models.ViewModels.SongVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class SongsController : ControllerBase
	{
		private readonly ISongRepository _repository;

		private readonly SongService _service;

		public SongsController(ISongRepository repository, IMemberRepository memberRepository)
		{
			_repository = repository;
			_service = new(_repository, memberRepository);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Popular")]
		public ActionResult<IEnumerable<SongIndexVM>> GetPopularSongs([FromQuery]int rowNumber)
		{
			var dtos = _repository.GetPopularSongs(0, "", rowNumber);

            return Ok(dtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{songName}")]
		public ActionResult<IEnumerable<SongIndexVM>> GetSongsByName(string songName, [FromQuery]int rowNumber = 2)
		{
			var dtos = _service.GetSongsByName(songName, rowNumber);

			return Ok(dtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("SongGenres")]
		public IActionResult GetSongGenres()
		{
			var songGenres = _repository.GetSongGenres();

			return Ok(songGenres);
		}

		[HttpGet]
		[Route("{songId}/Credits")]
		public IActionResult GetSongCredits(int songId)
		{
			var dto = _repository.GetSongById(songId);

			if (dto == null) return NotFound("歌曲不存在");

			var Credits = new SongCreditsVM
			{
				SongName = dto.SongName,
				Artists = dto.Artistlist.Select(list => list.ArtistName).ToList(),
				SongWriter = dto.SongWriter,
			};

			return Ok(Credits);
		}

		[HttpPost]
		[Route("{songId}/Records/{memberId}")]
		public IActionResult CreatePlayRecord(int songId, int memberId)
		{
			_service.CreatePlayRecord(songId, memberId);

            return Ok();
		}
	}
}
