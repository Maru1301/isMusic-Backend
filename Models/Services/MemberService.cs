using api.iSMusic.Models.DTOs.ActivityDTOs;
﻿using api.iSMusic.Models.DTOs.MemberDTOs;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.MemberVMs;
using BookStore.Site.Models.Infrastructures;
using iSMusic.Models.Infrastructures;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Runtime.InteropServices;
using System.Security.Claims;
using static api.iSMusic.Controllers.MembersController;
using static api.iSMusic.Controllers.QueuesController;

namespace api.iSMusic.Models.Services
{
    public class MemberService
	{
		private readonly IMemberRepository _memberRepository;

		private readonly IPlaylistRepository _playlistRepository;

		private readonly ISongRepository _songRepository;

		private readonly IArtistRepository _artistRepository;

		private readonly ICreatorRepository _creatorRepository;

		private readonly IAlbumRepository _albumRepository;

		private readonly IActivityRepository _activityRepository;

        private readonly IQueueRepository _queueRepository;

        public MemberService(IMemberRepository repo, IPlaylistRepository playlistRepository, ISongRepository songRepository, IArtistRepository artistRepository, ICreatorRepository creatorRepository, IAlbumRepository albumRepository, IActivityRepository activityRepository, IQueueRepository queueRepository)
		{
			_memberRepository = repo;
			_playlistRepository = playlistRepository;
			_songRepository = songRepository;
			_artistRepository = artistRepository;
			_creatorRepository = creatorRepository;
			_albumRepository = albumRepository;
			_activityRepository = activityRepository;
            _queueRepository = queueRepository;

        }

		public IEnumerable<PlaylistIndexDTO> GetMemberPlaylists(int memberId, InputQuery query)
		{
			var member = _memberRepository.GetMemberById(memberId);
			if (member == null)
			{
				return Enumerable.Empty<PlaylistIndexDTO>();
			}

			var playlists = _playlistRepository.GetMemberPlaylists(memberId, query);
			return playlists;
		}

		public (bool Success, string Message, IEnumerable<ArtistIndexDTO> ArtistDtos) GetLikedArtists(int memberId, LikedQuery query)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在", new List<ArtistIndexDTO>());

			var dtos = _artistRepository.GetLikedArtists(memberId, query);

			return (true, string.Empty, dtos);
		}

		public (bool Success, string Message, IEnumerable<CreatorIndexDTO> CreatorsDtos) GetLikedCreators(int memberId, LikedQuery query)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在", new List<CreatorIndexDTO>());

			var dtos = _creatorRepository.GetLikedCreators(memberId, query);

			return (true, "", dtos);
		}

		public (bool Success, string Message, IEnumerable<AlbumIndexDTO> AlbumsDtos) GetLikedAlbums(int memberId, LikedQuery query)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在", new List<AlbumIndexDTO>());

			var dtos = _albumRepository.GetLikedAlbums(memberId, query);

			return (true, "", dtos);
		}

		public IEnumerable<PlaylistIndexDTO> GetMemberPlaylistsByName(int memberId, string name, int rowNumber)
		{
			var member = _memberRepository.GetMemberById(memberId);
			if (member == null)
			{
				return Enumerable.Empty<PlaylistIndexDTO>();
			}

			var playlists = _playlistRepository.GetMemberPlaylistsByName(memberId, name, rowNumber);

			return playlists;
		}

        public (bool Success, string Message, IEnumerable<ActivityIndexDTO>Dtos) GetMemberFollowedActivities(int memberId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在", new List<ActivityIndexDTO>());

			var dtos = _activityRepository.GetMemberFollowedActivities(memberId);
			return (true, string.Empty, dtos);
        }

        public IEnumerable<SubscribeDetailDTO> GetSubscriptionDetail(int memberId)
        {
            return _memberRepository.GetSubscriptionDetail(memberId);
        }

        public (bool Success, string Message) AddLikedSong(int memberId, int songId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckSongExistence(songId) == false) return (false, "歌曲不存在");

			_memberRepository.AddLikedSong(memberId, songId);
			return (true, "成功新增");
		}

		public (bool Success, string Message) AddLikedPlaylist(int memberId, int playlistId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckPlaylistExistence(playlistId) == false) return (false, "播放清單不存在");

			_memberRepository.AddLikedPlaylist(memberId, playlistId);
			return (true, "成功新增");
		}

		public (bool Success, string Message) AddLikedAlbum(int memberId, int albumId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckAlbumExistence(albumId) == false) return (false, "專輯不存在");

			_memberRepository.AddLikedAlbum(memberId, albumId);
			return (true, "成功新增");
		}

		public (bool Success, string Message) FollowArtist(int memberId, int artistId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckArtistExistence(artistId) == false) return (false, "表演者不存在");

			_memberRepository.FollowArtist(memberId, artistId);
			return (true, "成功新增");
		}

        public (bool Success, string Message) FollowCreator(int memberId, int creatorId)
        {
            if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

            if (CheckCreatorExistence(creatorId) == false) return (false, "表演者不存在");

            _memberRepository.FollowCreator(memberId, creatorId);
            return (true, "成功新增");
        }

        public (bool Success, string Message) FollowActivity(int memberId, int activityId, DateTime attendDate)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");
			if(CheckActivityExistence(activityId) == false) return (false, "活動不存在");

            _activityRepository.FollowActivity(memberId, activityId, attendDate);
            return (true, "成功新增");
        }

        
        public (bool Success, string Message) DeleteLikedSong(int memberId, int songId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckSongExistence(songId) == false) return (false, "歌曲不存在");

			_memberRepository.DeleteLikedSong(memberId, songId);
			return (true, "成功刪除");
		}

		public (bool Success, string Message) DeleteLikedPlaylist(int memberId, int playlistId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckPlaylistExistence(playlistId) == false) return (false, "播放清單不存在");

			_memberRepository.DeleteLikedPlaylist(memberId, playlistId);
			return (true, "成功刪除");
		}

		public (bool Success, string Message) DeleteLikedAlbum(int memberId, int albumId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckAlbumExistence(albumId) == false) return (false, "專輯不存在");

			_memberRepository.DeleteLikedAlbum(memberId, albumId);
			return (true, "成功刪除");
		}

		public (bool Success, string Message) UnfollowArtist(int memberId, int artistId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckArtistExistence(artistId) == false) return (false, "表演者不存在");

			_memberRepository.UnfollowArtist(memberId, artistId);
			return (true, "成功刪除");
		}

        public (bool Success, string Message) UnfollowCreator(int memberId, int creatorId)
        {
            if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

            if (CheckCreatorExistence(creatorId) == false) return (false, "表演者不存在");

            _memberRepository.UnfollowCreator(memberId, creatorId);
            return (true, "成功刪除");
        }

        public (bool Success, string Message) UnfollowActivity(int memberId, int activityId)
        {
            if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

            if (CheckCreatorExistence(activityId) == false) return (false, "活動不存在");

            _activityRepository.UnfollowActivity(memberId, activityId);
            return (true, "成功刪除");
        }

        private bool CheckSongExistence(int songId)
		{
			var song = _songRepository.GetSongById(songId);

			return song != null;
		}

		private bool CheckMemberExistence(int memberId)
		{
			var member = _memberRepository.GetMemberById(memberId);

			return member != null;
		}

		private bool CheckPlaylistExistence(int playlistId)
		{
			var playlist = _playlistRepository.GetPlaylistByIdForCheck(playlistId);

			return playlist != null;
		}

		private bool CheckAlbumExistence(int albumId)
		{
			var playlist = _albumRepository.GetAlbumByIdForCheck(albumId);

			return playlist != null;
		}

		private bool CheckArtistExistence(int artistId)
		{
			var artist = _artistRepository.GetArtistByIdForCheck(artistId);

			return artist != null;
		}

		private bool CheckCreatorExistence(int creatorId)
		{
			var creator = _creatorRepository.GetCreatorByIdForCheck(creatorId);

			return creator != null;
		}

        private bool CheckActivityExistence(int activityId)
        {
            var activity = _activityRepository.GetActivityByIdForCheck(activityId);

            return activity != null;
        }

        public (bool Success, string Message) UpdateMember(int memberId, MemberDTO member)
        {
            var dto = _memberRepository.GetMemberById(memberId).ToDTO();
            if (dto == null) return (false, "此帳號不存在");
            // TODO 驗證修改的資料(暱稱是否存在...)
            member.Id = dto.Id;
            _memberRepository.UpdateMember(member);
            return (true, "更新成功");
        }
        public (bool Success, string Message) UpdateEmail(int memberId, string email)
        {
            var emailExist = _memberRepository.EmailExist(email);
            var dto = _memberRepository.GetMemberById(memberId).ToDTO();
            if (dto == null) return (false, "此帳號不存在");
            if (emailExist) return (false, "此信箱已存在");
            _memberRepository.UpdateEmail(dto, email);
            return (true, "更新成功");
        }

        public MemberDTO GetMemberInfo(int memberId)
        {
            // 用 memberId 取的資料庫資料
            return _memberRepository.GetMemberInfo(memberId)!;
        }

        public (bool Success, string Message) MemberRegister(MemberRegisterDTO dto, string urlTemplate)
        {
            if (_memberRepository.IsExist(dto.MemberAccount!))
            {
                return (false, "帳號已存在");
            }
            // 驗證暱稱是否重複
            if (_memberRepository.NickNameExist(dto.MemberNickName!))
            {
                return (false, "暱稱已存在");
            }

            if (_memberRepository.EmailExist(dto.MemberEmail!))
            {
                return (false, "信箱已存在");
            }

            dto.ConfirmCode = Guid.NewGuid().ToString("N");

            _memberRepository.MemberRegister(dto);

            MemberDTO entity = _memberRepository.GetByAccount(dto.MemberAccount);
            // 發email
            string url = string.Format(urlTemplate, entity.Id, dto.ConfirmCode);

            new EmailHelper().SendConfirmRegisterEmail(url, dto.MemberNickName!, dto.MemberEmail!);

            // 創建播放佇列
            _queueRepository.CreateQueue(entity.Id);

            return (true, "註冊成功，已發送驗證信");
        }

        public (bool Success, string Message) ActivateRegister(int memberId, string confirmCode)
        {
            MemberDTO dto = _memberRepository.Load(memberId);
            if (dto == null) return (false, "驗證成功");

            if (string.Compare(dto.ConfirmCode, confirmCode) != 0) return (false, "驗證成功");

            _memberRepository.ActivateRegister(memberId);
            return (true, "驗證成功");
        }

        public (bool Success, string? MemberNickName, ClaimsIdentity claimsIdentity) MemberLogin(MemberDTO dto)
        {
            MemberDTO member = _memberRepository.GetByAccount(dto.MemberAccount);

            if (member == null)
            {
                return (false, "帳密有誤", null!);
            }

            string encryptedPwd = HashUtility.ToSHA256(dto.MemberPassword, MemberRegisterDTO.SALT);

            if (String.CompareOrdinal(member.MemberEncryptedPassword, encryptedPwd) != 0) return (false, "帳密有誤", null!);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, member.MemberAccount),
                    new Claim("MemberId", member.Id.ToString()),      
                    
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return (true, member.MemberNickName, claimsIdentity);
        }

        public (bool Success, string? Message) RequestResetPassword(string email, string urlTemplate)
        {
            var dto = _memberRepository.GetByEmail(email);

            if (dto == null)
            {
                return (false, "帳號或 Email 錯誤");
            }

            if (string.Compare(email, dto.MemberEmail) != 0)
            {
                return (false, "帳號或 Email 錯誤");
            }

            ////檢查 IsConfirmed必需是true
            //if (dto.IsConfirmed == false)
            //{
            //    return (false, "您還沒有啟用本帳號, 請先完成才能重設密碼");
            //}

            // 更新記錄, 填入 confirmCode
            string confirmCode = Guid.NewGuid().ToString("N");
            dto.ConfirmCode = confirmCode;
            _memberRepository.UpdateMember(dto);

            // 發email
            string url = string.Format(urlTemplate, dto.Id, confirmCode);

            new EmailHelper().SendForgetPasswordEmail(url, dto.MemberAccount, email);
            return (true, "已重新發送信件");
        }

        public (bool Success, string? Message) ResetPassword(int memberId, string confirmCode, string plainTextPassword)
        {
            // todo 檢查傳入參數值是否合理


            string encryptedPassword = HashUtility.ToSHA256(plainTextPassword, MemberRegisterDTO.SALT);

            MemberDTO entity = _memberRepository.Load(memberId);
            // 檢查有沒有記錄
            if (entity == null) throw new Exception("找不到對應的會員記錄");

            // 檢查confirmcode 
            if (string.Compare(confirmCode, entity.ConfirmCode) != 0)
            {
                throw new Exception("找不到對應的會員記錄");
            }

            // 更新密碼
            _memberRepository.UpdatePassword(memberId, encryptedPassword);

            return (true, "重設密碼成功");
        }

        public IEnumerable<MemberSubscriptionPlanDTO> GetMemberSubscriptionPlan(int memberId)
        {
            //if (_memberRepository.SubscriptionRecordExist(memberId) == null)
            //{
            //    return ("找不到訂閱紀錄");
            //}
            return _memberRepository.GetMemberSubscriptionPlan(memberId)!;
        }

        public IEnumerable<SubscriptionPlanDTO> GetSubscriptionPlan()
        {
            return _memberRepository.GetSubscriptionPlan();
        }

        public (bool Success, string Message) SubscribedPlan(int memberId, SubscribedPlanVM model)
        {
            var SubscriptionPlan = _memberRepository.SubscriptionPlanLoad(model.SubscriptionPlanId);
            // TODO 如果有並過期 傳回訂閱已到期訊息
            //if (dto.SubscribedExpireTime < DateTime.Now) return (false, "訂閱已到期")            
            var date = DateTime.Now;
            var member = _memberRepository.Load(memberId);
            if (member.CreditCardId == null) return (false, "請先輸入信用卡資訊");
            if (memberId == 0) return (false, "找不到對應的會員記錄");
            if (SubscriptionPlan.Id == 0) return (false, "找不到對應的訂閱記錄");

            if (model.Emails == null) model.Emails = Enumerable.Empty<string>();
            if (SubscriptionPlan.NumberOfUsers != model.Emails.Count()+1)
            {
                return (false, $"email數量不符");
            }
            if (_memberRepository.SubscriptionRecordExist(memberId)) return (false, "已經訂閱過了");

            _memberRepository.CreateSubscribedPlanRecord(memberId, SubscriptionPlan, date);
            var subscriptionRecord = _memberRepository.GetSubscriptionRecords(memberId);
            _memberRepository.CreateSubscriptionRecordDetail(subscriptionRecord.Id, memberId);

            foreach (var email in model.Emails)
            {
                var memberForSubscrptionPlan = _memberRepository.GetByEmail(email);

                if (memberForSubscrptionPlan == null) return (false, "輸入信箱中存在非會員信箱");
                _memberRepository.CreateSubscriptionRecordDetail(subscriptionRecord.Id, memberForSubscrptionPlan.Id);
            }
            return (true, "訂閱成功");
        }   

        public IEnumerable<OrderDTO> GetMemberOrder(int memberId)
        {
            return _memberRepository.GetMemberOrder(memberId)!;
        }

        public(bool Success, string Message) ResendConfirmCode(int memberId, string email, string urlTemplate)
        {
            var dto = _memberRepository.GetMemberById(memberId)!.ToDTO();
            string confirmCode = Guid.NewGuid().ToString("N");
            dto!.ConfirmCode = confirmCode;
            
            //if (_memberRepository.EmailExist(newEmail))
            //{
            //    return (false, "信箱已存在");
            //}
            _memberRepository.UpdateEmail(dto, email);
            string url = string.Format(urlTemplate, memberId, confirmCode);
            new EmailHelper().ReSendConfirmEmail(url, dto.MemberAccount, email);
            return (true, "已重新發送信件");

        }
    }
}
