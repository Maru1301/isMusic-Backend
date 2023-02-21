using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.DTOs.MemberDTOs;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.Services.Interfaces
{
	public interface IMemberRepository
	{
		Member? GetMemberById(int memberId);

		Member? FindMemberById(int memberId);


        Task<Member?> GetMemberAsync(int memberId);

		void AddLikedSong(int memberId, int songId);

		void AddLikedPlaylist(int memberId, int playlistId);

		void DeleteLikedSong(int memberId, int songId);

		void DeleteLikedPlaylist(int memberId, int playlistId);

		void UpdateMember(int memberId, MemberDTO memberDTO);

		MemberDTO? GetMemberInfo(int memberId);

		void MemberRegister(MemberRegisterDTO source);

		bool IsExist(string account, string nickName);

		void ActiveRegister(int memberId);

    }
}
