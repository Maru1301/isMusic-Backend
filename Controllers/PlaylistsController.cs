using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using API_practice.Models.ViewModels.PlaylistVMs;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PlaylistsController : ControllerBase
	{
		private readonly AppDbContext _db;

		public PlaylistsController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		[Route("Recommended")]
		public IActionResult GetRecommended()
		{
			var recommendedPlaylists = _db.Playlists
				.Where(p => p.IsPublic)
				.Include(p => p.LikedPlaylists)
				.Select(p => new PlaylistIndexVM
				{
					Id = p.Id,
					ListName = p.ListName,
					PlaylistCoverPath = p.PlaylistCoverPath,
					MemberId = p.MemberId,
					TotalLikes = p.LikedPlaylists.Count(),
				})
				.OrderByDescending(x => x.TotalLikes)
				.Take(10)
				.ToList();
				
			if(recommendedPlaylists.Count() == 0)
			{
				return NoContent();
			}

			return Ok(recommendedPlaylists);
		}

		[HttpGet]
		[Route("{playlistId}")]
		public ActionResult<PlaylistDetailVM> GetPlaylistDetail(int playlistId)
		{
			if (playlistId <= 0)
			{
				return BadRequest("Invalid playlist id");
			}

			var playlist = _db.Playlists
				.Include(p => p.PlaylistSongMetadata)
				.Single(p => p.Id == playlistId);

			if (playlist == null)
			{
				return NotFound("Playlist not found");
			}

			var memberId = _db.Members.Single(member => member.Id == playlist.MemberId).Id;

			var likedSongIds = _db.LikedSongs
				.Where(l => l.MemberId == memberId)
				.Select(l => l.SongId)
				.ToList();

			var songs = _db.Songs
				.Include(s => s.Album)
				.Where(s => playlist.PlaylistSongMetadata.Select(m => m.SongId).Contains(s.Id))
				.ToList();

			var playlistDetailVM = playlist.ToDetailVM();

			foreach (var metadata in playlistDetailVM.PlayListSongMetadata)
			{
				var song = songs.Single(s => s.Id == metadata.SongId).ToInfoVM();
				if (likedSongIds.Contains(song.Id))
				{
					song.IsLiked = true;
				}
				metadata.Song = song;
			}

			return playlistDetailVM;
		}

		[HttpGet]
		[Route("Search")]
		public ActionResult<IEnumerable<PlaylistIndexVM>> Search([FromQuery] SearchQuery query)
		{
			var data = _db.Playlists.Where(playlist => playlist.ListName.Contains(query.Input) || playlist.MemberId == query.MemberId);

			if (query.ShowAll == false)
			{
				data = data.Where(playlist => playlist.ListName.Contains(query.Input));
			}

			return Ok(data.Select(p => p.ToIndexVM()));
		}

		public class SearchQuery
		{
			public string Input { get; set; } = null!;

			public bool ShowAll { get; set; }

			public int MemberId { get; set; }
		}

		[HttpPut]
		[Route("{playlistId}")]
		public async Task<IActionResult> EditPlaylistDetail(int playlistId, [FromForm] PlaylistEditVM model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			//Find the playlist in the database
			var playlist = await _db.Playlists.FirstOrDefaultAsync(p => p.Id == playlistId);

			if (playlist == null)
			{
				return NotFound("Playlist not found");
			}

			//Update the playlist with the data from the view model
			playlist.ListName = model.ListName;
			playlist.Description = model.Description;

			if (model.PlaylistCover != null)
			{
				var fileName = Path.GetFileName(model.PlaylistCover.FileName);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await model.PlaylistCover.CopyToAsync(stream);
				}

				playlist.PlaylistCoverPath = Path.Combine("uploads", fileName);
			}

			//Save the changes to the database
			await _db.SaveChangesAsync();

			return NoContent();
		}

		[HttpPost]
		[Route("Song")]
		public IActionResult AddSongintoPlaylist([FromBody] AddSongRequest request)
		{
			var playlist = _db.Playlists.Find(request.PlaylistId);
			if (playlist == null)
			{
				return NotFound("Playlist not found");
			}

			var lastMetadata = _db.PlaylistSongMetadata
				.Where(m => m.PlayListId == request.PlaylistId)
				.OrderByDescending(m => m.DisplayOrder)
				.FirstOrDefault();

			var lastOrder = (lastMetadata != null) ? lastMetadata.DisplayOrder : 0;

			var metadata = new PlaylistSongMetadatum
			{
				SongId = request.SongId,
				PlayListId = request.PlaylistId,
				DisplayOrder = lastOrder + 1
			};

			_db.PlaylistSongMetadata.Add(metadata);
			_db.SaveChanges();

			return CreatedAtAction(nameof(GetPlaylistDetail), new { request.PlaylistId }, playlist.ToIndexVM());
		}

		public class AddSongRequest
		{
			public int SongId { get; set; }
			public int PlaylistId { get; set; }
		}

		[HttpDelete]
		[Route("Song")]
		public IActionResult DeleteSongfromPlaylist(int playlistId, int songId)
		{
			var metadata = _db.PlaylistSongMetadata
				.FirstOrDefault(m => m.PlayListId == playlistId && m.SongId == songId);

			if (metadata == null)
			{
				return NotFound("Playlist-song metadata not found");
			}

			_db.PlaylistSongMetadata.Remove(metadata);
			_db.SaveChanges();

			return NoContent();
		}
	}
}
