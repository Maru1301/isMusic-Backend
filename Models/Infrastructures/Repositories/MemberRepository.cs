using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.DTOs.MemberDTOs;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
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

        public MemberDTO Load(int memberId)
        {
            Member entity = _db.Members.SingleOrDefault(x => x.Id == memberId)!;
            if (entity == null) return null!;

            MemberDTO dto = new MemberDTO
            {
                Id = entity.Id,
                MemberAccount = entity.MemberAccount,
                MemberEmail = entity.MemberEmail,
                IsConfirmed = entity.IsConfirmed,
                ConfirmCode = entity.ConfirmCode
            };

            return dto;
        }

        public SubscriptionPlanDTO SubscriptionPlanLoad(int SubscriptionPlanId)
        {
            SubscriptionPlan entity = _db.SubscriptionPlans.SingleOrDefault(s => s.Id == SubscriptionPlanId)!;
            if (entity == null) return null!;

            SubscriptionPlanDTO dto = new SubscriptionPlanDTO
            {
                PlanName = entity.PlanName,
                Price = entity.Price,
                NumberOfUsers = entity.NumberOfUsers,                
            };
            return dto;
        }

        public bool SubscriptionRecordExist(int memberId)
        {
            var entity = _db.SubscriptionRecords.Where(s => s.Member.Id == memberId).SingleOrDefault();

            return (entity != null);
        }

        public bool NickNameExist(string nickName)
        {
            var entity = _db.Members.Where(m => m.MemberNickName == nickName).SingleOrDefault();

            return (entity != null);
        }

        public bool EmailExist(string email)
        {
            var entity = _db.Members.Where(m => m.MemberEmail == email).SingleOrDefault();

            return (entity != null);
        }

        public Member? GetMemberById(int memberId)
		{
			return _db.Members.SingleOrDefault(m => m.Id == memberId);
		}

        public Member? GetByEmail(string email)
        {
            return _db.Members.SingleOrDefault(m => m.MemberEmail == email);
        }

        public MemberDTO GetByAccount(string Account)
        {
            var data = _db.Members
                .SingleOrDefault(x => x.MemberAccount == Account)!;

            return data.ToDTO();
        }

        public bool IsExist(string account)
		{			
            var entity = _db.Members.Where(m => m.MemberAccount == account).SingleOrDefault();

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
            member.MemberAddress = memberDTO.MemberAddress;
            member.MemberCellphone = memberDTO.MemberCellphone;
            member.MemberDateOfBirth = memberDTO.MemberDateOfBirth;
            member.Avatar = memberDTO.Avatar;
            member.MemberReceivedMessage = memberDTO.MemberReceivedMessage;
            member.MemberSharedData = memberDTO.MemberSharedData;
            member.LibraryPrivacy = memberDTO.LibraryPrivacy;
            member.CalenderPrivacy = memberDTO.CalenderPrivacy;            

            _db.SaveChanges();
        }

        public MemberDTO? GetMemberInfo(int memberId)
        {
            // 得到的 memberId 跟資料庫做比較，如果符合取出那筆資料的值
            var result = _db.Members.Where(m => m.Id == memberId).Include(m => m.Avatar)
                .Select(member => new MemberDTO  // 將取到的值轉成DTO
                {
                    Id = memberId,
                    MemberNickName = member.MemberNickName,
                    MemberEmail = member.MemberEmail,
                    MemberAccount = member.MemberAccount,
                    MemberAddress = member.MemberAddress,
                    MemberCellphone = member.MemberCellphone,
                    MemberDateOfBirth = member.MemberDateOfBirth,
                    MemberReceivedMessage = member.MemberReceivedMessage,
                    MemberSharedData = member.MemberSharedData,
                    LibraryPrivacy = member.LibraryPrivacy,
                    CalenderPrivacy = member.CalenderPrivacy,
                    Avatar = member.Avatar,
                }).SingleOrDefault();

            return result;
        }

        public void MemberRegister(MemberRegisterDTO dto)
        {
            var member = new Member
            {
                MemberAccount = dto.MemberAccount!,
                MemberEncryptedPassword = dto.MemberEncryptedPassword!,
                MemberNickName = dto.MemberNickName!,
                MemberEmail = dto.MemberEmail!,
                IsConfirmed = false, //預設是未確認的會員
                ConfirmCode = dto.ConfirmCode
            };
            _db.Members.Add(member);
            _db.SaveChanges();
        }

        public void ActivateRegister(int memberId)
        {
            var member = _db.Members.Find(memberId)!;
            member.IsConfirmed = true;
            member.ConfirmCode = null;
            _db.SaveChanges();
        }

        public void UpdatePassword(int memberId, string newEncryptedPassword)
        {
            var member = _db.Members.Find(memberId);

            member!.MemberEncryptedPassword = newEncryptedPassword;
            // 將confirmCode清空
            member.ConfirmCode = null;

            _db.SaveChanges();
        }

        public IEnumerable<SubscriptionPlanDTO> GetMemberSubscriptionPlan(int memberId)
        {
            // 得到的 memberId 跟資料庫做比較，如果符合取出那筆資料的值
            var result = _db.SubscriptionRecords
                .Include(s => s.Member)
                .Include(s => s.SubscriptionPlan)
                .Include(s => s.Member.Avatar)
                .Where(s => s.MemberId == memberId)
                .Select(s => new SubscriptionPlanDTO  // 將取到的值轉成DTO
                {
                    MemberId = memberId,
                    MemberNickName = s.Member.MemberNickName,                    
                    SubscribedTime = s.SubscribedTime,
                    PlanName = s.SubscriptionPlan.PlanName,
                    Price = s.SubscriptionPlan.Price,
                    NumberOfUsers = s.SubscriptionPlan.NumberOfUsers,                    
                })
                .ToList();

            return result;
        }

        public void SubscribedPlan(int memberId, SubscriptionPlanDTO dto, IEnumerable<MemberDTO> memberdto, DateTime addDate)
        {
            var SubscriptionRecords = new SubscriptionRecord
            {
                MemberId = memberId,
                SubscriptionPlanId = dto.Id,
                SubscribedTime = DateTime.Now,
                SubscribedExpireTime= addDate,
            };
            _db.SubscriptionRecords.Add(SubscriptionRecords);
            _db.SaveChanges();
        }

        public IEnumerable<OrderDTO> GetMemberOrder(int memberId)
        {
            var result = _db.OrderProductMetadata
                .Include(o => o.Product)
                .Include(o => o.Order)
                .ThenInclude(o => o.Member)
                .Include(o => o.Order.Coupon)
                .Select(o => new OrderDTO
                {
                    MemberId = memberId,
                    MemberNickName = o.Order.Member.MemberNickName,
                    CouponText = o.Order.Coupon.CouponText,
                    StartDate = o.Order.Coupon.StartDate,
                    ExpiredDate = o.Order.Coupon.ExpiredDate,
                    Discounts = o.Order.Coupon.Discounts,
                    Payments = o.Order.Payments,
                    OrderStatus = o.Order.OrderStatus,
                    Paid = o.Order.Paid,
                    Created = o.Order.Created,
                    Receiver = o.Order.Receiver,
                    Address = o.Order.Address,
                    Cellphone = o.Order.Cellphone,
                    CategoryName = o.Product.ProductCategory.CategoryName,
                    ProductName = o.ProductName,
                    Price = o.Price,
                    Qty = o.Qty,
                    Status = o.Product.Status,                    
                });
            return result;
        }        
    }
}
