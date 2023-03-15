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
                CreditCardId = entity.CreditCardId,
                IsConfirmed = entity.IsConfirmed,
                ConfirmCode = entity.ConfirmCode
            };

            return dto;
        }

        public MemberSubscriptionPlanDTO SubscriptionPlanLoad(int SubscriptionPlanId)
        {
            SubscriptionPlan entity = _db.SubscriptionPlans.SingleOrDefault(s => s.Id == SubscriptionPlanId)!;
            if (entity == null) return null!;

            MemberSubscriptionPlanDTO dto = new MemberSubscriptionPlanDTO
            {
                Id = entity.Id,
                PlanName = entity.PlanName,
                Price = entity.Price,
                NumberOfUsers = entity.NumberOfUsers,                
            };
            return dto;
        }

        public bool SubscriptionRecordExist(int memberId)
        {
            var entity = _db.SubscriptionRecords.Where(s => s.MemberId == memberId).SingleOrDefault();

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

        public MemberDTO? GetByEmail(string email)
        {
            var data = _db.Members.SingleOrDefault(m => m.MemberEmail == email);
            return data!.ToDTO();
        }

        public MemberDTO GetByAccount(string Account)
        {
            var data = _db.Members
                .Single(x => x.MemberAccount == Account);

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

		public void AddLikedAlbum(int memberId, int albumId)
		{
			var data = new LikedAlbum
			{
				MemberId = memberId,
				AlbumId = albumId,
				Created = DateTime.Now,
			};

			_db.LikedAlbums.Add(data);
			_db.SaveChanges();
		}

		public void DeleteLikedAlbum(int memberId, int albumId)
		{
			var data = _db.LikedAlbums.SingleOrDefault(la => la.MemberId == memberId && la.AlbumId == albumId);

			if (data == null) throw new Exception("此歌曲不在喜歡列表內");

			_db.LikedAlbums.Remove(data);
			_db.SaveChanges();
		}

		public void FollowArtist(int memberId, int artistId)
		{
			var data = new ArtistFollow
			{
				MemberId = memberId,
				ArtistId = artistId,
				Created = DateTime.Now,
			};

			_db.ArtistFollows.Add(data);
			_db.SaveChanges();
		}

		public void UnfollowArtist(int memberId, int artistId)
		{
			var data = _db.ArtistFollows.SingleOrDefault(af => af.MemberId == memberId && af.ArtistId == artistId);

			if (data == null) throw new Exception("此歌曲不在喜歡列表內");

			_db.ArtistFollows.Remove(data);
			_db.SaveChanges();
		}

        public void FollowCreator(int memberId, int creatorId)
        {
            var data = new CreatorFollow
            {
                MemberId = memberId,
                CreatorId = creatorId,
                Created = DateTime.Now,
            };

            _db.CreatorFollows.Add(data);
            _db.SaveChanges();
        }

        public void UnfollowCreator(int memberId, int creatorId)
        {
            var data = _db.CreatorFollows.SingleOrDefault(cf => cf.MemberId == memberId && cf.CreatorId == creatorId);

            if (data == null) throw new Exception("此歌曲不在喜歡列表內");

            _db.CreatorFollows.Remove(data);
            _db.SaveChanges();
        }

        public void UpdateMember(MemberDTO memberDTO)
        {
            // 將 DTO 的值修改到資料庫
            var member = _db.Members.Single(m => m.Id == memberDTO.Id);

            member.MemberNickName = memberDTO.MemberNickName;            
            member.MemberAddress = memberDTO.MemberAddress;
            member.MemberCellphone = memberDTO.MemberCellphone;
            member.MemberDateOfBirth = memberDTO.MemberDateOfBirth;
            //member.AvatarId = memberDTO.AvatarId;
            member.MemberReceivedMessage = memberDTO.MemberReceivedMessage;
            member.MemberSharedData = memberDTO.MemberSharedData;
            member.LibraryPrivacy = memberDTO.LibraryPrivacy;
            member.CalenderPrivacy = memberDTO.CalenderPrivacy;
            member.ConfirmCode = memberDTO.ConfirmCode;

            _db.SaveChanges();
        }

        public void UpdateEmail(MemberDTO dto, string email)
        {
            var member = _db.Members.Single(m => m.Id == dto.Id);
            member.ConfirmCode = dto.ConfirmCode;
            member.MemberEmail = email;

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
                    IsConfirmed= member.IsConfirmed,
                    AvatarId = member.Avatar!.Id,
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

        public IEnumerable<MemberSubscriptionPlanDTO> GetMemberSubscriptionPlan(int memberId)
        {
            // 得到的 memberId 跟資料庫做比較，如果符合取出那筆資料的值
            var result = _db.SubscriptionRecords
                .Include(s => s.Member)
                .Include(s => s.SubscriptionPlan)
                .Include(s => s.Member.Avatar)
                .Where(s => s.MemberId == memberId)
                .Select(s => new MemberSubscriptionPlanDTO  // 將取到的值轉成DTO
                {
                    MemberId = memberId,
                    MemberNickName = s.Member.MemberNickName,                    
                    SubscribedTime = s.SubscribedTime,
                    SubscribedExpireTime = s.SubscribedExpireTime,
                    PlanName = s.SubscriptionPlan.PlanName,
                    Price = s.SubscriptionPlan.Price,
                    NumberOfUsers = s.SubscriptionPlan.NumberOfUsers,                    
                })
                .ToList();

            return result;
        }

        public void CreateSubscribedPlanRecord(int memberId, MemberSubscriptionPlanDTO dto, DateTime date)
        {
            var SubscriptionRecords = new SubscriptionRecord
            {
                MemberId = memberId,
                SubscriptionPlanId = dto.Id,
                SubscribedTime = date,
                SubscribedExpireTime= date.AddMonths(1),
            };
            _db.SubscriptionRecords.Add(SubscriptionRecords);
            _db.SaveChanges();
        }

        public void CreateSubscriptionRecordDetail(int subscriptionRecordId, int memberForSubscrptionPlanId)
        {
            var RecordDetail = new SubscriptionRecordDetail
            {
                SubscriptionRecordId = subscriptionRecordId,
                MemberId = memberForSubscrptionPlanId,
            };
            _db.SubscriptionRecordDetails.Add(RecordDetail);
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

        public SubscriptionRecordsDTO GetSubscriptionRecords(int memberId)
        {
            var data = _db.SubscriptionRecords.SingleOrDefault(s => s.MemberId == memberId);

            return data.ToSubscriptionRecordsDTO();
        }

        public IEnumerable<SubscriptionPlanDTO> GetSubscriptionPlan()
        {
            var result = _db.SubscriptionPlans.Select(s => new SubscriptionPlanDTO
            {
                Id = s.Id,
                PlanName = s.PlanName,
                Price = s.Price,
                NumberOfUsers = s.NumberOfUsers,
                Description = s.Description
            });
            return result;
        }


        public IEnumerable<SubscribeDetailDTO> GetSubscriptionDetail(int memberId)
        {
            var result = _db.SubscriptionRecordDetails
                .Include(s => s.Member)
                .Include(s => s.SubscriptionRecord)
                .ThenInclude(s => s.SubscriptionPlan)
                .Where(s => s.MemberId == memberId)
                .Select(s => new SubscribeDetailDTO
                {
                    MemberNickName = s.Member.MemberNickName,
                    //MemberEmail = s.Member.MemberEmail,
                    NumberOfUsers = s.SubscriptionRecord.SubscriptionPlan.NumberOfUsers,
                    PlanName = s.SubscriptionRecord.SubscriptionPlan.PlanName,
                    Description = s.SubscriptionRecord.SubscriptionPlan.Description
                });
            return result;
        }

    }
}
