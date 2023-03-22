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
using System.ComponentModel.DataAnnotations;
using api.iSMusic.Models.Infrastructures.Repositories;
using System.Globalization;
using System;

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

		private readonly ICreatorRepository _creatorRepository;

        public MembersController(IMemberRepository memberRepo, ISongRepository songRepository, IArtistRepository artistRepository, ICreatorRepository creatorRepository, IPlaylistRepository playlistRepository, IAlbumRepository albumRepository, IQueueRepository queueRepository, IActivityRepository activityRepository)
		{
			_memberRepository = memberRepo;
			_songRepository = songRepository;
			_playlistRepository = playlistRepository;
			_queueRepository = queueRepository;
			_creatorRepository = creatorRepository;
            _memberService = new(_memberRepository, _playlistRepository, _songRepository, artistRepository, creatorRepository, albumRepository, activityRepository, queueRepository);			
        }

        [HttpPost]
        [Route("Register")]
		[AllowAnonymous]
        public IActionResult MemberRegister([FromBody] MemberRegisterVM member)
        {
            // email驗證網址
            string urlTemplate = "http://localhost:8080/member.html#/MemberActivate/{0}/{1}";


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
				return NotFound(result.MemberNickName);
			}

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(result.claimsIdentity));
            return Ok(result.MemberNickName);
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
            string urlTemplate = "http://localhost:8080/resetPassword.html#/ResetPassword/{0}/{1}";

            var result = _memberService.RequestResetPassword(email, urlTemplate);

            return Ok(result.Message);
        }

        [HttpPatch]        
        [Route("ResetPassword")]
		[AllowAnonymous]
		public IActionResult ResetPassword([FromQuery] int memberId, string confirmCode, [FromBody] MemberResetPasswordVM source)
        {

            var result = _memberService.ResetPassword(memberId, confirmCode, source.Password);
            return Ok(result.Message);
        }

		[HttpGet]
		[Route("SubscriptionRecord")]
		public IEnumerable<MemberSubscriptionPlanDTO> GetMemberSubscriptionPlan()
		{			
			var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);
            var result = _memberService.GetMemberSubscriptionPlan(memberId);

            return result;

        }

        [HttpGet]
        [Route("SubscriptionPlan")]
        [AllowAnonymous]
        public IEnumerable<SubscriptionPlanDTO> GetSubscriptionPlan()
        {
            var result = _memberService.GetSubscriptionPlan();

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

		public class UpdateEmail
		{
            [Required]
            [EmailAddress]
            public string Email { get; set; } = null;
        }
        [HttpPatch]
		[Route("UpdateEmail")]
		public IActionResult UdateMemberEmail([FromForm] UpdateEmail email)
		{
            var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _memberService.UpdateEmail(memberId, email.Email);
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
            _creatorRepository.CreateCreator(memberId);

            return Ok(result.Message);
        }

		[HttpPost]
		[Route("SubscribePlan")]
		public IActionResult SubscribedPlan([FromForm] SubscribedPlanVM model)
		{
			var memberId = this.GetMemberId();
      
			var result = _memberService.SubscribedPlan(memberId, model);
			return Ok(result.Message);
        }

		[HttpPatch]
		[Route("ResendConfirmCode")]
		public IActionResult ResendConfirmCode([FromForm] string email)
		{
            var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);

            string urlTemplate = "http://localhost:8080/member.html#/memberActivate/{0}/{1}";
            var result = _memberService.ResendConfirmCode(memberId, email, urlTemplate);
            return Ok(result.Message);
        }

		[HttpGet]
		[Route("SubscribeDetails")]
		public IEnumerable<SubscribeDetailDTO> GetSubscribeDetail()
		{
			var memberId = int.Parse(HttpContext.User.FindFirst("MemberId")!.Value);

			var result = _memberService.GetSubscriptionDetail(memberId);

			return result;
		}

		[HttpGet]
		[Route("Playlists")]
		public IActionResult GetMemberPlaylists([FromQuery] InputQuery query)
		{
            int memberId = this.GetMemberId();
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
		[Route("Playlists/{playlistName}")]
		public IActionResult GetMemberPlaylistsByName(string name, [FromQuery] int rowNumber = 2)
		{
            int memberId = this.GetMemberId();
            var dto = _memberService.GetMemberPlaylistsByName(memberId, name, rowNumber);

			return Ok(dto);
		}

		[HttpGet]
		[Route("Queue")]
		public async Task<IActionResult> GetMemberQueue()
		{
			var memberId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "MemberId")!.Value);
			try
			{
				var member = await _memberRepository.GetMemberAsync(memberId);
				if (member == null)
				{
					return NotFound("會員不存在");
				}

				var queue = await _queueRepository.GetQueueByMemberIdAsync(memberId);
				if (queue == null)
				{
					return NotFound("佇列不存在");
				}

				var queueSongIds = queue.SongInfos.Select(info => info.Id);

				var likedSongIds = _songRepository.GetLikedSongIdsByMemberId(memberId);

				foreach (int songId in queueSongIds)
				{
					if (likedSongIds.Contains(songId))
					{
                        queue.SongInfos
							.Where(info => info.Id == songId)
							.ToList()
							.ForEach(info => info.IsLiked = true);
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
		[Route("RecentlyPlayed")]
		public IActionResult GetRecentlyPlayed()
		{
            int memberId = this.GetMemberId();
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
		[Route("LikedArtists")]
		public IActionResult GetLikedArtists([FromQuery] LikedQuery query)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.GetLikedArtists(memberId, query);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.ArtistDtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("LikedCreators")]
		public IActionResult GetLikedCreators([FromQuery] LikedQuery query)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.GetLikedCreators(memberId, query);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.CreatorsDtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("LikedAlbums")]
		public IActionResult GetLikedAlbums([FromQuery] LikedQuery query)
		{
            int memberId = this.GetMemberId();
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
		[Route("Activities")]
		public IActionResult GetMemberFollowedActivities()
		{
            int memberId = this.GetMemberId();
            var result = _memberService.GetMemberFollowedActivities(memberId);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Dtos.Select(dto => dto.ToIndexVM()));

        }
		[HttpPost]
		[Route("LikedSongs/{songId}")]
		public IActionResult AddLikedSong(int songId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.AddLikedSong(memberId, songId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("LikedPlaylists/{playlistId}")]
		public IActionResult AddLikedPlaylist(int playlistId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.AddLikedPlaylist(memberId, playlistId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("LikedAlbums/{albumId}")]
		public IActionResult AddLikedAlbum(int albumId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.AddLikedAlbum(memberId, albumId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("FollowedArtists/{artistId}")]
		public IActionResult FollowArtist(int artistId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.FollowArtist(memberId, artistId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("FollowedCreators/{creatorId}")]
		public IActionResult FollowCreator(int creatorId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.FollowCreator(memberId, creatorId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpPost]
		[Route("Activities/{activityId}/{attendDate}")]
		public IActionResult FollowActivity([FromRoute] int activityId, [FromRoute] DateTime attendDate)
		{
			
			int memberId = this.GetMemberId();
			//DateTime attendDateTime = DateTime.ParseExact(attendDate, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
			var result = _memberService.FollowActivity(memberId, activityId, attendDate);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("LikedSongs/{songId}")]
		public IActionResult DeleteLikedSong(int songId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.DeleteLikedSong(memberId, songId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("LikedPlaylists/{playlistId}")]
		public IActionResult DeleteLikedPlaylist(int playlistId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.DeleteLikedPlaylist(memberId, playlistId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("LikedAlbums/{albumId}")]
		public IActionResult DeleteLikedAlbum(int albumId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.DeleteLikedAlbum(memberId, albumId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("FollowedArtists/{artistId}")]
		public IActionResult UnfollowArtist(int artistId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.UnfollowArtist(memberId, artistId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("FollowedCreators/{creatorId}")]
		public IActionResult UnfollowCreator(int creatorId)
		{
            int memberId = this.GetMemberId();
            var result = _memberService.UnfollowCreator(memberId, creatorId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}

		[HttpDelete]
		[Route("Activities/{activityId}")]
		public IActionResult UnfollowActivity(int activityId)
		{
			int memberId = this.GetMemberId();
			var result = _memberService.UnfollowActivity(memberId, activityId);

			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			return Ok(result.Message);
		}
	}
}
