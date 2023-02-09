using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
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
		private readonly AppDbContext _db;

		public SongsController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		[Route("Popular")]
		public ActionResult<IEnumerable<SongIndexVM>> GetPopularSongs()
		{
			var data = _db.Songs
				.Where(s => s.AlbumId != null && s.Status != false)
				.OrderByDescending(s => s.SongPlayedRecords.Count())
				.Take(10)
				.Select(s => new SongIndexVM
				{
					Id = s.Id,
					SongName = s.SongName,
					GenreName = s.Genre.GenreName,
					IsExplicit = s.IsExplicit,
					SongCoverPath = s.SongCoverPath,
					SongPath = s.SongPath,
					AlbumId = s.AlbumId != null? s.AlbumId.Value : 0,
					PlayedTimes = s.SongPlayedRecords.Count(),
					Artistlist = s.SongArtistMetadata.Select(m => m.Artist.ToInfoVM()).ToList(),
					Creatorlist = s.SongCreatorMetadata.Select(m => m.Creator.ToInfoVM()).ToList(),
				});

			return Ok(data);
		}

		[HttpGet]
		[Route("Search")]
		public ActionResult<IEnumerable<SongIndexVM>> Search([FromQuery] string input)
		{
			var data = _db.Songs.Where(song => song.SongName.Contains(input) && song.Status == true);

			return Ok(data);
		}

		[HttpGet]
		[Route("RecentlyPlayed")]
		public ActionResult<IEnumerable<SongIndexVM>> GetRecentlyPlayedSongs([FromQuery] string memberAccount)
		{
			var member = _db.Members.SingleOrDefault(member => member.MemberAccount == memberAccount);

			if (member == null)
			{
				return NotFound("Member does not existed");
			}

			var data = _db.SongPlayedRecords.Where(record => record.MemberId == member.Id).Include(record => record.Song).Select(record => record.Song);

			if (data == null)
			{
				return NoContent();
			}

			return Ok(data);
		}
	}
}
