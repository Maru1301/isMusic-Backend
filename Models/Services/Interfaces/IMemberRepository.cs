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

		void DeleteLikedSong(int memberId, int songId);

		void DeleteLikedPlaylist(int memberId, int playlistId);

        Member? GetByEmail(string email);

        MemberDTO GetByAccount(string Account);

        void UpdateMember(int memberId, MemberDTO memberDTO);

        MemberDTO? GetMemberInfo(int memberId);

        MemberDTO Load(int memberId);

        SubscriptionPlanDTO SubscriptionPlanLoad(int SubscriptionPlanId);

        void MemberRegister(MemberRegisterDTO source);

        bool IsExist(string account);

        bool NickNameExist(string nickName);

        bool EmailExist(string email);

        bool SubscriptionRecordExist(int memberId);

        void ActivateRegister(int memberId);

        void UpdatePassword(int memberId, string newEncryptedPassword);

        IEnumerable<SubscriptionPlanDTO> GetMemberSubscriptionPlan(int memberId);

        void SubscribedPlan(int memberId, SubscriptionPlanDTO subscriptionPlan, IEnumerable<MemberDTO> memberdto, DateTime addDate);

        IEnumerable<OrderDTO> GetMemberOrder(int memberId);
    }
}
