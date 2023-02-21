using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.DTOs.MemberDTOs;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
	public class MemberRepository: IRepository, IMemberRepository
	{
		private readonly AppDbContext _db;

		public MemberRepository(AppDbContext db)
		{
			_db = db;
		}

		public Member? GetMemberById(int memberId)
		{
			return _db.Members.SingleOrDefault(m => m.Id == memberId);
		}
		public Member? FindMemberById(int memberId)
		{
			return _db.Members.Find(memberId);
		}

        public MemberDTO GetByAccount(string Account)
        {
            var data = _db.Members
                .SingleOrDefault(x => x.MemberAccount == Account)!;

            return data.ToDTO();
        }

        public bool IsExist(string account, string nickName)
		{			
            var entity = _db.Members.Where(m => m.MemberAccount == account || m.MemberNickName == nickName).SingleOrDefault();

            return (entity != null);
        }

        public async Task<Member?> GetMemberAsync(int memberId)
		{
			return await _db.Members.SingleOrDefaultAsync(m => m.Id == memberId);
		}

		public void AddLikedSong(int memberId, int songId)
		{
			var data = new LikedSong
			{
				MemberId = memberId,
				SongId= songId,
				Created= DateTime.Now,
			};

			_db.LikedSongs.Add(data);
			_db.SaveChanges();
		}

		public void DeleteLikedSong(int memberId, int songId)
		{
			var data = _db.LikedSongs.SingleOrDefault(ls => ls.MemberId== memberId && ls.SongId == songId);

			if (data == null) throw new Exception("此歌曲不在喜歡列表內");

			_db.LikedSongs.Remove(data);
			_db.SaveChanges();
		}

		public void AddLikedPlaylist(int memberId, int playlistId)
		{
			var data = new LikedPlaylist
			{
				MemberId = memberId,
				PlaylistId = playlistId,
				Created = DateTime.Now,
			};

			_db.LikedPlaylists.Add(data);
			_db.SaveChanges();
		}

		public void DeleteLikedPlaylist(int memberId, int playlistId)
		{
			var data = _db.LikedPlaylists.SingleOrDefault(lp => lp.MemberId == memberId && lp.PlaylistId == playlistId);

			if (data == null) throw new Exception("此歌曲不在喜歡列表內");

			_db.LikedPlaylists.Remove(data);
			_db.SaveChanges();
		}

		public void UpdateMember(int memberId, MemberDTO memberDTO)
		{
			// 將 DTO 的值修改到資料庫
			var member = _db.Members.Single(m => m.Id == memberId);
			
			member.MemberNickName = memberDTO.MemberNickName;
			member.MemberEmail = memberDTO.MemberEmail;
			member.MemberAddress = memberDTO.MemberAddress;
			member.MemberCellphone = memberDTO.MemberCellphone;
			member.MemberDateOfBirth = memberDTO.MemberDateOfBirth;
			//Avatar = memberDTO.Avatar,
			member.MemberReceivedMessage = memberDTO.MemberReceivedMessage;
			member.MemberSharedData = memberDTO.MemberSharedData;
			member.LibraryPrivacy = memberDTO.LibraryPrivacy;
			member.CalenderPrivacy = memberDTO.CalenderPrivacy;
			// 信用卡?

			_db.SaveChanges();
		}

		public MemberDTO? GetMemberInfo(int memberId)
		{
			// 得到的 memberId 跟資料庫做比較，如果符合取出那筆資料的值
            var result = _db.Members.Where(m => m.Id == memberId).Include(m=>m.Avatar)
				.Select(member => new MemberDTO  // 將取到的值轉成DTO
			{				
				MemberNickName = member.MemberNickName,
				MemberEmail= member.MemberEmail,
				MemberAccount= member.MemberAccount,
				MemberAddress= member.MemberAddress,
				MemberCellphone= member.MemberCellphone,
				MemberDateOfBirth= member.MemberDateOfBirth,				
				MemberReceivedMessage= member.MemberReceivedMessage,
				MemberSharedData= member.MemberSharedData,				
				LibraryPrivacy= member.LibraryPrivacy,
				CalenderPrivacy= member.CalenderPrivacy,
				Avatar= member.Avatar,
			}).SingleOrDefault();

            return result;
        }

		public void MemberRegister(MemberRegisterDTO dto)
		{
			var member = new Member
			{
				MemberAccount = dto.MemberAccount!,
				MemberEncryptedPassword= dto.MemberEncryptedPassword!,
                MemberNickName = dto.MemberNickName!,
				MemberEmail= dto.MemberEmail!,
				IsConfirmed= false, //預設是未確認的會員
                ConfirmCode = dto.ConfirmCode
            };
            _db.Members.Add(member);
            _db.SaveChanges();
        }

        public void ActiveRegister(int memberId)
        {
            var member = _db.Members.Find(memberId)!;
            member.IsConfirmed = true;
            member.ConfirmCode = null;
            _db.SaveChanges();
        }

    }
}
