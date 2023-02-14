using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
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
		private readonly IPlaylistRepository _repository;

		private readonly ISongRepository _songRepository;

		private readonly PlaylistService _service;

		public PlaylistsController(IPlaylistRepository repo, ISongRepository songRepository)
		{
			_repository = repo;
			_songRepository = songRepository;
			_service = new(_repository, _songRepository);
		}

		[HttpGet]
		[Route("Recommended")]
		public IActionResult GetRecommended()
		{
			var recommendedPlaylists = _service.GetRecommended();
				
			if(!recommendedPlaylists.Any())
			{
				return NoContent();
			}

			return Ok(recommendedPlaylists);
		}

		[HttpGet]
		[Route("{playlistId}")]
		public ActionResult<PlaylistDetailVM> GetPlaylistDetail(int playlistId)
		{
			var result = _service.GetPlaylistDetail(playlistId);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}
			return result.PlaylistDetail;
		}

		/// <summary>
		/// Use input name to search the playlists
		/// </summary>
		/// <param name="playlistName">input name</param>
		/// <param name="rowNumber">the default number is 1</param>
		/// <returns>a list of playlists</returns>
		[HttpGet]
		[Route("{playlistName}")]
		public ActionResult<IEnumerable<PlaylistIndexVM>> GetPlaylistsByName([FromRoute] string playlistName, [FromQuery]int rowNumber = 2)
		{
			var data = _service.GetPlaylistsByName(playlistName, rowNumber);

			return Ok(data.Select(p => p.ToIndexVM()));
		}

		[HttpPost]
		[Route("{playlistId}/Songs/{songId}")]
		public IActionResult AddSongToPlaylist(int playlistId, int songId, [FromBody]bool Force)
		{
			var result = _service.AddSongToPlaylist(playlistId, songId, Force);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPut]
		[Route("{playlistId}/Detail")]
		public async Task<IActionResult> UpdatePlaylistDetail(int playlistId, [FromForm] PlaylistEditVM model)
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

			return Ok();
		}

		//[HttpDelete]
		//[Route("Song")]
		//public IActionResult DeleteSongfromPlaylist(int playlistId, int songId)
		//{
		//	var metadata = _db.PlaylistSongMetadata
		//		.FirstOrDefault(m => m.PlayListId == playlistId && m.SongId == songId);

		//	if (metadata == null)
		//	{
		//		return NotFound("Playlist-song metadata not found");
		//	}

		//	_db.PlaylistSongMetadata.Remove(metadata);
		//	_db.SaveChanges();

		//	return NoContent();
		//}
	}
}
