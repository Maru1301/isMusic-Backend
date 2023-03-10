using api.iSMusic.Models;
using api.iSMusic.Models.DTOs.MemberDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.MemberVMs;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using api.iSMusic.Models.ViewModels.QueueVMs;
using iSMusic.Models.Infrastructures;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using BookStore.Site.Models.Infrastructures;

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

		private readonly MemberService _memberService;		
        
        public MembersController(IMemberRepository memberRepo, ISongRepository songRepository, IArtistRepository artistRepository, ICreatorRepository creatorRepository, IPlaylistRepository playlistRepository, IAlbumRepository albumRepository, IQueueRepository queueRepository, IActivityRepository activityRepository)
		{
			_memberRepository = memberRepo;
			_songRepository = songRepository;
			_playlistRepository = playlistRepository;
			_queueRepository = queueRepository;
			_memberService = new(_memberRepository, _playlistRepository, _songRepository, artistRepository, creatorRepository, albumRepository, activityRepository, queueRepository);			
        }

        [HttpPost]
        [Route("Register")]
		[AllowAnonymous]
        public IActionResult MemberRegister([FromForm] MemberRegisterVM member)
        {
            // email驗證網址
            string urlTemplate = Request.Scheme + "://" + Request.Host + Url.Content("~/") + "Members/ActivateRegister?memberid={0}&confirmCode={1}";


            var result = _memberService.MemberRegister(member.ToMemberDTO(), urlTemplate);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost]
        [Route("MemberLogin")]
		[AllowAnonymous]
		public IActionResult MemberLogin([FromBody]MemberLoginVM member)
        {
            var result = _memberService.MemberLogin(member.ToLoginDTO());

			if (!result.Success)
			{
				return NotFound(result.Message);
			}

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(result.claimsIdentity));
            return Ok(result.Message);
        }

        [HttpPost("MemberLogOut")]
        public IActionResult MemberLogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Ok("登出成功");
        }

        [HttpGet]
        [Route("ForgetPassword")]
		[AllowAnonymous]
		public IActionResult ForgetPassword(string email)
        {
            string urlTemplate = Request.Scheme + "://" + Request.Host + Url.Content("~/") + "Members/ResetPassword?memberid={0}&confirmCode={1}";

            var result = _memberService.RequestResetPassword(email, urlTemplate);

            return Ok(result.Message);
        }

        [HttpPatch]        
        [Route("ResetPassword")]
		[AllowAnonymous]
		public IActionResult ResetPassword([FromQuery] int memberId, string confirmCode, [FromForm] MemberResetPasswordVM source)
        {

            var result = _memberService.ResetPassword(memberId, confirmCode, source.Password);
            return Ok(result.Message);
        }

		[HttpGet]
		[Route("SubscriptionPlan")]
		public IEnumerable<SubscriptionPlanDTO> GetMemberSubscriptionPlan()
		{			
			var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);
            var result = _memberService.GetMemberSubscriptionPlan(memberId);

            return result;

        }

		[HttpGet]
		[Route("Orders")]
		public IEnumerable<OrderDTO> GetMemberOrder()
		{
            var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);
            var result = _memberService.GetMemberOrder(memberId);

			return result;
		}

        [HttpGet]
        //[Route("{memberId}")]
        public IActionResult GetMemberInfo()
        {
            // 取得 memberId
            var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);
            var member = _memberService.GetMemberInfo(memberId);
            if (member == null)
            {
                return NotFound("Member not found");
            }

            return Ok(member);
        }

        [HttpPut]
        //[Route("{memberId}")]
        public IActionResult UpdateMember([FromForm] MemberEditVM member)
        {
            var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _memberService.UpdateMember(memberId, member.ToMemberDTO());

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpGet]
        [Route("ActivateRegister")]
        [AllowAnonymous]
        public IActionResult ActivateRegister(int memberId, string confirmCode)
        {            
            var result = _memberService.ActivateRegister(memberId, confirmCode);
			if (!result.Success)
			{
				return Forbid(result.Message);
			}

            return Ok(result.Message);
        }

		[HttpPost]
		[Route("SubscribePlan")]
		public IActionResult SubscribedPlan([FromForm] SubscribedPlanVM source)
		{
			var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);
			var SubscriptionPlan = _memberRepository.SubscriptionPlanLoad(source.SubscriptionPlanId);
      
			var result = _memberService.SubscribedPlan(memberId, SubscriptionPlan, source.Emails);
			return Ok(result.Message);
        }

		[HttpPatch]
		[Route("ResendConfirmCode")]
		public IActionResult ResendConfirmCode([FromForm] string newEmail)
		{
            var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);

            string urlTemplate = Request.Scheme + "://" + Request.Host + Url.Content("~/") + "Members/ActivateRegister?memberid={0}&confirmCode={1}";
            var result = _memberService.ResendConfirmCode(memberId, newEmail, urlTemplate);
            return Ok(result.Message);
        }













		[HttpGet]
		[Route("{memberId}/Playlists")]
		public ActionResult<IEnumerable<PlaylistIndexVM>> GetMemberPlaylists([FromRoute] int memberId, [FromQuery] InputQuery query)
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
				Value = string.Empty;
			}
			public int RowNumber { get; set; }

			public bool IncludedLiked { get; set; }

			public string Condition { get; set; }

			public string Value { get; set; }
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

				foreach (int songId in queueSongIds)
				{
					if (likedSongIds.Contains(songId))
					{
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

			return Ok(result.ArtistDtos.Select(dto => dto.ToIndexVM()));
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
				Input = string.Empty;
			}
			public int RowNumber { get; set; }

			public string Condition { get; set; }

			public string Input { get; set; }
		}

		[HttpGet]
		[Route("{memberId}/Activities")]
		public IActionResult GetMemberFollowedActivities(int memberId)
		{
			var result = _memberService.GetMemberFollowedActivities(memberId);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Dtos.Select(dto => dto.ToIndexVM()));

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

		[HttpPost]
		[Route("{memberId}/FollowedArtists/{artistId}")]
		public IActionResult FollowArtist(int memberId, int artistId)
		{
			var result = _memberService.FollowArtist(memberId, artistId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("{memberId}/FollowedCreators/{creatorId}")]
		public IActionResult FollowCreator(int memberId, int creatorId)
		{
			var result = _memberService.FollowCreator(memberId, creatorId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("{memberId}/Activities/{activityId}/{attendDate}")]
		public IActionResult FollowActivity(int memberId, int activityId, DateTime attendDate)
		{
			var result = _memberService.FollowActivity(memberId, activityId, attendDate);

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

		[HttpDelete]
		[Route("{memberId}/FollowedArtists/{artistId}")]
		public IActionResult UnfollowArtist(int memberId, int artistId)
		{
			var result = _memberService.UnfollowArtist(memberId, artistId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("{memberId}/FollowedCreators/{creatorId}")]
		public IActionResult UnfollowCreator(int memberId, int creatorId)
		{
			var result = _memberService.UnfollowCreator(memberId, creatorId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("{memberId}/Activities/{activityId}")]
		public IActionResult UnfollowActivity(int memberId, int activityId)
		{
			var result = _memberService.UnfollowActivity(memberId, activityId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}
	}
}
