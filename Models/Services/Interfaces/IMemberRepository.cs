using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.DTOs.MemberDTOs;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.Services.Interfaces
{
	public interface IMemberRepository
	{
		Member? GetMemberById(int memberId);

        Task<Member?> GetMemberAsync(int memberId);

		void AddLikedSong(int memberId, int songId);

		void AddLikedPlaylist(int memberId, int playlistId);

		void AddLikedAlbum(int memberId, int albumId);

		void FollowArtist(int memberId, int artistId);

        void FollowCreator(int memberId, int creatorId);

        void DeleteLikedSong(int memberId, int songId);

		void DeleteLikedPlaylist(int memberId, int playlistId);

		void DeleteLikedAlbum(int memberId, int albumId);

		void UnfollowArtist(int memberId, int artistId);

		void UnfollowCreator(int memberId, int creatorId);

        MemberDTO? GetByEmail(string email);

        MemberDTO GetByAccount(string Account);

        void UpdateMember(MemberDTO memberDTO);

        MemberDTO? GetMemberInfo(int memberId);

        MemberDTO Load(int memberId);

        MemberSubscriptionPlanDTO SubscriptionPlanLoad(int SubscriptionPlanId);

        void MemberRegister(MemberRegisterDTO source);

        bool IsExist(string account);

        bool NickNameExist(string nickName);

        bool EmailExist(string email);

        bool SubscriptionRecordExist(int memberId);

        void ActivateRegister(int memberId);

        void UpdatePassword(int memberId, string newEncryptedPassword);

        IEnumerable<MemberSubscriptionPlanDTO> GetMemberSubscriptionPlan(int memberId);

        void CreateSubscribedPlanRecord(int memberId, MemberSubscriptionPlanDTO subscriptionPlan, DateTime addDate);

        void CreateSubscriptionRecordDetail(int subscriptionRecordId, int memberForSubscrptionPlanId);

        IEnumerable<OrderDTO> GetMemberOrder(int memberId);

        void UpdateEmail(MemberDTO dto, string email);

        SubscriptionRecordsDTO GetSubscriptionRecords(int memberId);

        IEnumerable<SubscribeDetailDTO> GetSubscriptionDetail(int memberId);

        IEnumerable<SubscriptionPlanDTO> GetSubscriptionPlan();

    }
}
