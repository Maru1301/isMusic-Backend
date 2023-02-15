using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.SongVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ArtistsController : ControllerBase
	{
		private readonly IArtistRepository _repository;
		
		private readonly ArtistService _service;

		public ArtistsController(IArtistRepository repository)
		{
			_repository = repository;
			_service = new ArtistService(_repository);
		}

		[HttpGet]
		[Route("{artistId}/Detail")]
		public ActionResult<ArtistDetailVM> GetArtistDetail(int artistId)
		{
			var result = _service.GetArtistDetail(artistId);
			if (!result.Success)
			{
				return NotFound(result.Message);
			}

			var data = _db.Artists.SingleOrDefault(artist => artist.Id == artistId);

			if (data == null) return NotFound();

			var popularSongs = _db.Songs
				.Include(song => song.SongArtistMetadata)
				.Include(song => song.SongPlayedRecords)
				.Where(song => song.SongArtistMetadata.Select(metadata => metadata.ArtistId).Contains(artistId))
				.Select(song => new
				{
					song.Id,
					song.SongName,

				});

			return Ok(data.ToDetailVM());
		}

		[HttpGet]
		[Route("{artistName}")]
		public ActionResult<IEnumerable<ArtistIndexVM>> GetArtistsByArtistName(string artistName, [FromQuery]int rowNumber = 2)
		{
			var dtos = _service.GetArtistsByName(artistName, rowNumber);

			return Ok(dtos.Select(dto => dto.ToIndexVM()));
		}
	}
}
