using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using api.iSMusic.Models.ViewModels.QueueVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class MembersController : ControllerBase
	{
		private readonly AppDbContext _db;

		public MembersController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		[Route("{memberId}/Playlists")]
		public ActionResult<IEnumerable<PlaylistIndexVM>> GetMemberPlaylist([FromRoute] int memberId, [FromQuery] bool myOwn)
		{
			var data = _db.Playlists;
			List<int> likedPlaylists = new List<int>();

			var member = _db.Members.SingleOrDefault(m => m.Id == memberId);

			if (member == null)
			{
				return NotFound("Member not found");
			}

			IQueryable<Playlist> playlists;
			if (!myOwn)
			{
				likedPlaylists = _db.LikedPlaylists
					.Where(lp => lp.MemberId == memberId)
					.Select(lp => lp.PlaylistId)
					.ToList();

				playlists = data.Where(p => p.MemberId == memberId || likedPlaylists.Contains(p.Id));
			}
			else
			{
				playlists = data.Where(p => p.MemberId == memberId);
			}

			return Ok(playlists.Select(p => p.ToIndexVM()));
		}

		[HttpGet]
		[Route("{memberId}/Queue")]
		public ActionResult<QueueIndexVM> GetMemberQueue([FromRoute] int memberId)
		{
			var member = _db.Members.SingleOrDefault(m => m.Id == memberId);

			if (member == null)
			{
				return NotFound(new { message = "Member not found" });
			}

			var queue = _db.Queues.Include(q => q.CurrentSong)
								.Include(q => q.QueueSongs)
								.SingleOrDefault(q => q.MemberId == memberId);

			if (queue == null)
			{
				return NotFound(new { message = "Queue not found for the given member" });
			}

			return Ok(queue.ToIndexVM());
		}

		[HttpPost]
		[Route("{memberId}/Playlist")]
		public async Task<IActionResult> CreatePlaylist([FromRoute]int memberId)
		{
			//Check if the provided memberAccount is valid
			if (memberId <= 0)
			{
				return BadRequest("Invalid member account");
			}

			// Find the number of playlists created by the member
			var numOfPlaylists = await _db.Playlists.CountAsync(p => p.MemberId == memberId);
			numOfPlaylists += 1;

			//Create a new playlist
			var newPlaylist = new PlaylistCreateVM
			{
				MemberId = memberId,
				ListName = "MyPlaylist" + numOfPlaylists
			};

			//Add the new playlist to the database
			_db.Playlists.Add(newPlaylist.ToEntity());
			await _db.SaveChangesAsync();

			var playlistId = _db.Playlists.Where(p => p.MemberId == memberId).OrderByDescending(p => p.Id).First().Id;

			//Return a 201 Created status code along with the newly created playlist's information
			return CreatedAtAction("GetPlaylistDetail", "Playlists", new { playlistId }, newPlaylist);
		}
	}
}
