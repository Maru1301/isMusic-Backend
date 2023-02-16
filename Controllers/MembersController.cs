using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
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
		private readonly IMemberRepository _memberRepository;

		private readonly ISongRepository _songRepository;

		private readonly IPlaylistRepository _playlistRepository;

		private readonly IQueueRepository _queueRepository;

		private readonly IAlbumRepository _albumRepository;

		private readonly MemberService _memberService;

		public MembersController(IMemberRepository memberRepo, ISongRepository songRepository, IArtistRepository artistRepository, ICreatorRepository creatorRepository, IPlaylistRepository playlistRepository, IAlbumRepository albumRepository , IQueueRepository queueRepository)
		{
			_memberRepository = memberRepo;
			_songRepository = songRepository;
			_playlistRepository = playlistRepository;
			_queueRepository = queueRepository;
			_albumRepository= albumRepository;
			_memberService = new (_memberRepository, _playlistRepository, _songRepository, artistRepository, creatorRepository, albumRepository);
		}

		[HttpGet]
		[Route("{memberId}/Playlists")]
		public ActionResult<IEnumerable<PlaylistIndexVM>> GetMemberPlaylists([FromRoute] int memberId, [FromQuery]InputQuery query)
		{
			var dtos = _memberService.GetMemberPlaylists(memberId, query);

			if (dtos == null)
			{
				return NotFound("Member not found");
			}

			return Ok(dtos.Select(dto => dto.ToIndexVM()));
		}

		public class InputQuery
		{
			public InputQuery()
			{
				RowNumber = 2;
				IncludedLiked = true;
				Condition = "RecentlyAdded";
			}
			public int RowNumber { get; set; }

			public bool IncludedLiked { get; set; }

			public string Condition { get; set; }
		}

		[HttpGet]
		[Route("{memberId}/Playlists/{playlistName}")]
		public IActionResult GetMemberPlaylistsByName(int memberId, string name, [FromQuery] int rowNumber = 2)
		{
			var dto = _memberService.GetMemberPlaylistsByName(memberId, name, rowNumber);

			return Ok(dto);
		}

		[HttpGet]
		[Route("{memberId}/Queue")]
		public async Task<IActionResult> GetMemberQueue([FromRoute] int memberId)
		{
			try
			{
				var member = await _memberRepository.GetMemberAsync(memberId);
				if (member == null)
				{
					return NotFound(new { message = "Member not found" });
				}

				var queue = await _queueRepository.GetQueueByMemberIdAsync(memberId);
				if (queue == null)
				{
					return NotFound(new { message = "Queue not found for the given member" });
				}

				var queueSongIds = queue.SongInfos.Select(info => info.Id);

				var likedSongIds = _songRepository.GetLikedSongIdsByMemberId(memberId);

				foreach(int songId in queueSongIds)
				{
					if (likedSongIds.Contains(songId)){
						queue.SongInfos.Single(info => info.Id == songId).IsLiked = true;
					}
				}

				return Ok(queue.ToIndexVM());
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet]
		[Route("{memberId}/RecentlyPlayed")]
		public IActionResult GetRecentlyPlayed(int memberId)
		{
			//Check if the provided memberAccount is valid
			if (memberId <= 0)
			{
				return BadRequest("Invalid member account");
			}

			var _songService = new SongService(_songRepository, _memberRepository);

			var result = _songService.GetRecentlyPlayed(memberId);

			if (!result.Success)
			{
				return NotFound(result.ErrorMessage);
			}

			return Ok(result.RecentlyPlayedSongs.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{memberId}/LikedArtists")]
		public IActionResult GetLikedArtists(int memberId, [FromQuery] LikedQuery query)
		{
			var result = _memberService.GetLikedArtists(memberId, query);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.ArtistDtos.Select(dto=> dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{memberId}/LikedCreators")]
		public IActionResult GetLikedCreators(int memberId, [FromQuery] LikedQuery query)
		{
			var result = _memberService.GetLikedCreators(memberId, query);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.CreatorsDtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{memberId}/LikedAlbums")]
		public IActionResult GetLikedAlbums(int memberId, [FromQuery] LikedQuery query)
		{
			var result = _memberService.GetLikedAlbums(memberId, query);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.AlbumsDtos.Select(dto => dto.ToIndexVM()));
		}

		public class LikedQuery
		{
			public LikedQuery()
			{
				RowNumber = 2;
				Condition = "RecentlyAdded";
			}
			public int RowNumber { get; set; }

			public string Condition { get; set; }
		}

		[HttpPost]
		[Route("{memberId}/Playlist")]
		public async Task<IActionResult> CreatePlaylist([FromRoute] int memberId)
		{
			//Check if the provided memberAccount is valid
			if (memberId <= 0)
			{
				return BadRequest("Invalid member account");
			}

			var _playlistService = new PlaylistService(_playlistRepository, _songRepository, _albumRepository);

			var playlistId = await _playlistService.CreatePlaylistAsync(memberId);

			//Return a 201 Created status code along with the newly created playlist's information
			return Ok(playlistId);
		}

		[HttpPost]
		[Route("{memberId}/LikedSongs/{songId}")]
		public IActionResult AddLikedSong(int memberId, int songId)
		{
			var result = _memberService.AddLikedSong(memberId, songId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("{memberId}/LikedPlaylists/{playlistId}")]
		public IActionResult AddLikedPlaylist(int memberId, int playlistId)
		{
			var result = _memberService.AddLikedPlaylist(memberId, playlistId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("{memberId}/LikedAlbums/{albumId}")]
		public IActionResult AddLikedAlbum(int memberId, int albumId)
		{
			var result = _memberService.AddLikedAlbum(memberId, albumId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("{memberId}/LikedSongs/{songId}")]
		public IActionResult DeleteLikedSong(int memberId, int songId)
		{
			var result = _memberService.DeleteLikedSong(memberId, songId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("{memberId}/LikedPlaylists/{playlistId}")]
		public IActionResult DeleteLikedPlaylist(int memberId, int playlistId)
		{
			var result = _memberService.DeleteLikedPlaylist(memberId, playlistId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("{memberId}/LikedAlbums/{albumId}")]
		public IActionResult DeleteLikedAlbum(int memberId, int albumId)
		{
			var result = _memberService.DeleteLikedAlbum(memberId, albumId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}
	}
}
