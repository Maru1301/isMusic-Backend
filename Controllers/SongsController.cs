using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using api.iSMusic.Models.ViewModels.SongVMs;
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

		public SongsController(ISongRepository repository)
		{
			_repository = repository;
			_service = new(_repository);
		}

		[HttpGet]
		[Route("Popular")]
		public ActionResult<IEnumerable<SongIndexVM>> GetPopularSongs()
		{
			var data = _repository.GetPopularSongs();

			return Ok(data.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{songName}")]
		public ActionResult<IEnumerable<SongIndexVM>> Search([FromRoute]string songName)
		{
			var dtos = _repository.SearchBySongName(songName);

			return Ok(dtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("SongGenres")]
		public IActionResult GetSongGenres()
		{
			var songGenres = _repository.GetSongGenres();

			return Ok(songGenres);
		}

		//[HttpGet]
		//[Route("RecentlyPlayed")]
		//public ActionResult<IEnumerable<SongIndexVM>> GetRecentlyPlayedSongs([FromQuery] string memberAccount)
		//{
		//	var member = _db.Members.SingleOrDefault(member => member.MemberAccount == memberAccount);

		//	if (member == null)
		//	{
		//		return NotFound("Member does not existed");
		//	}

		//	var data = _db.SongPlayedRecords.Where(record => record.MemberId == member.Id).Include(record => record.Song).Select(record => record.Song);

		//	if (data == null)
		//	{
		//		return NoContent();
		//	}

		//	return Ok(data);
		//}
	}
}
