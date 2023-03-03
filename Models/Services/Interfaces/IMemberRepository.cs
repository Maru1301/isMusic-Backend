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

        Member? GetByEmail(string email);

        MemberDTO GetByAccount(string Account);

        void UpdateMember(int memberId, MemberDTO memberDTO);

        MemberDTO? GetMemberInfo(int memberId);

        MemberDTO Load(int memberId);

        void MemberRegister(MemberRegisterDTO source);

        bool IsExist(string account);

        bool NickNameExist(string nickName);

        bool EmailExist(string email);

        bool SubscriptionRecordExist(int memberId);

        void ActiveRegister(int memberId);

        void UpdatePassword(int memberId, string newEncryptedPassword);

        IEnumerable<SubscriptionPlanDTO> GetMemberSubscriptionPlan(int memberId);

        IEnumerable<OrderDTO> GetMemberOrder(int memberId);
    }
}
