using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.ArtistVMs;
using API_practice.Models.ViewModels.SongVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_practice.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArtistController : ControllerBase
	{
		private readonly AppDbContext _db;

		public ArtistController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		[Route("Detail")]
		public ActionResult<ArtistDetailVM> GetArtistDetail([FromQuery]int artistId)
		{
			var data = _db.Artists.SingleOrDefault(artist =>artist.Id== artistId);

			if(data==null) return NotFound();

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
