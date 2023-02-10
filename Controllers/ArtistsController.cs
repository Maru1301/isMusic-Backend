using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.SongVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArtistsController : ControllerBase
	{
		private readonly AppDbContext _db;

		public ArtistsController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		[Route("Detail")]
		public ActionResult<ArtistDetailVM> GetArtistDetail([FromQuery] int artistId)
		{
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
		[Route("Search")]
		public ActionResult<IEnumerable<ArtistIndexVM>> Search([FromQuery] string input)
		{
			var data = _db.Artists.Where(artist => artist.ArtistName.Contains(input));

			return Ok(data.Select(artist => artist.ToIndexVM()));
		}
	}
}
