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

		public PlaylistsController(IPlaylistRepository repo, ISongRepository songRepository,IAlbumRepository albumRepository, IWebHostEnvironment webHostEnvironment)
		{
			_repository = repo;
			_songRepository = songRepository;
			_service = new(_repository, _songRepository, albumRepository, webHostEnvironment);
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

			return Ok(recommendedPlaylists.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{playlistId}")]
		public IActionResult GetPlaylistDetail(int playlistId)
		{
			int memberId = this.GetMemberId();
			var result = _service.GetPlaylistDetail(playlistId, memberId);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}
			return Ok(result.Dto.ToDetailVM());
		}

		/// <summary>
		/// Use input name to search the playlists
		/// </summary>
		/// <param name="playlistName">input name</param>
		/// <param name="rowNumber">the default number is 1</param>
		/// <returns>a list of playlists</returns>
		[HttpGet]
		[Route("Search/{playlistName}")]
		public ActionResult<IEnumerable<PlaylistIndexVM>> GetPlaylistsByName([FromRoute] string playlistName, [FromQuery]int rowNumber = 2)
		{
			var data = _service.GetPlaylistsByName(playlistName, rowNumber);

			return Ok(data.Select(p => p.ToIndexVM()));
		}

		private int GetMemberId()
		{
            return Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type == "MemberId").Value);
        }

        [HttpPost]
        [Route("NewList")]
        public async Task<IActionResult> CreatePlaylist()
        {
			int memberId = GetMemberId();
            //Check if the provided memberAccount is valid
            if (memberId <= 0)
            {
                return BadRequest("Invalid member account");
            }

            var playlistId = await _service.CreatePlaylistAsync(memberId);

            //Return a 201 Created status code along with the newly created playlist's information
            return Ok(playlistId);
        }
		
		public class ForceMode
		{
            public bool Value { get; set; }
        }

        [HttpPost]
		[Route("{playlistId}/Songs/{songId}")]
		public IActionResult AddSongToPlaylist(int playlistId, int songId, [FromBody]ForceMode Mode)
		{
			var result = _service.AddSongToPlaylist(playlistId, songId, Mode.Value);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}else if(result.Success == true && string.IsNullOrEmpty(result.Message))
			{
				return Accepted("歌曲已在清單中");
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("{playlistId}/Albums/{albumId}")]
		public IActionResult AddAlbumToPlaylist(int playlistId, int albumId, [FromBody]string mode = "Normal")
		{
			var result = _service.AddAlbumToPlaylist(playlistId, albumId, mode);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPut]
		[Route("{playlistId}/Detail")]
		public IActionResult UpdatePlaylistDetail(int playlistId, [FromForm]PlaylistEditVM model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = _service.UpdatePlaylistDetail(playlistId, model.ToEditDTO());

			if (!result.Success)
			{
				return NotFound(result.Messgae);
			}

			return Ok(result.Messgae);
		}

		[HttpPatch]
		[Route("{playlistId}/PrivacySetting")]
		public IActionResult ChangePrivacySetting(int playlistId)
		{
			var result = _service.ChangePrivacySetting(playlistId);
			if (!result.Success)
			{
				return NotFound(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("{playlistId}")]
		public IActionResult DeletePlaylist(int playlistId)
		{
			var result = _service.DeletePlaylist(playlistId);
			if (!result.Success)
			{
				return NotFound(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("{playlistId}/Songs/{displayOrder}")]
		public IActionResult DeleteSongfromPlaylist(int playlistId, int displayOrder)
		{
			var result = _service.DeleteSongfromPlaylist(playlistId, displayOrder);
			if (!result.Success)
			{
				return NotFound(result.Message);
			}

			return Ok(result.Message);
		}
	}
}
