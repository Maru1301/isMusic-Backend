using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.PlaylistVMs;
using API_practice.Models.ViewModels.SongVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_practice.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class SongController : ControllerBase
	{
		private readonly AppDbContext _db;

		public SongController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		[Route("Popular")]
		public ActionResult<IEnumerable<SongIndexVM>> GetPopularSongs()
		{
			var data = _db.Songs.Where(s => s.AlbumId != null && s.Status != false)
				.Include(s => s.SongPlayedRecords)
				.Include(s => s.SongArtistMetadata)
				.Include(s => s.SongCreatorMetadata)
				.Select(s => new
				{
					s.Id,
					s.SongName,
					GenreName = s.Genre.GenreName,
					s.IsExplicit,
					s.SongCoverPath,
					s.SongPath,
					s.AlbumId,
					PlayedTimes = s.SongPlayedRecords.Count(),
					Artists = s.SongArtistMetadata.Select(m => m.Artist),
					Creators = s.SongCreatorMetadata.Select(m => m.Creator),
				}).OrderByDescending(x => x.PlayedTimes).Take(10).ToList().Select(x => new SongIndexVM
				{
					Id = x.Id,
					SongName = x.SongName,
					GenreName = x.GenreName,
					IsExplicit = x.IsExplicit,
					SongCoverPath = x.SongCoverPath,
					SongPath = x.SongPath,
					AlbumId = x.AlbumId.Value,
					PlayedTimes = x.PlayedTimes,
					Artistlist = x.Artists.Select(a => a.ToInfoVM()).ToList(),
					Creatorlist = x.Creators.Select(c=>c.ToInfoVM()).ToList(),
				});

			return Ok(data);
		}

		[HttpGet]
		[Route("Search")]
		public ActionResult<IEnumerable<SongIndexVM>> Search([FromQuery]string input)
		{
			var data = _db.Songs.Where(song => song.SongName.Contains(input) && song.Status == true);

			return Ok(data);
		}

		[HttpGet]
		[Route("RecentlyPlayed")]
		public ActionResult<IEnumerable<SongIndexVM>> GetRecentlyPlayedSongs([FromQuery]string memberAccount)
		{
			var member = _db.Members.SingleOrDefault(member => member.MemberAccount== memberAccount);

			if(member == null)
			{
				return NotFound("Member does not existed");
			}

			var data = _db.SongPlayedRecords.Where(record => record.MemberId == member.Id).Include(record => record.Song).Select(record => record.Song);

			if(data == null)
			{
				return NoContent();
			}

			return Ok(data);
		}
	}
}
