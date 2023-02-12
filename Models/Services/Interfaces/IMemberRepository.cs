using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.Services.Interfaces
{
	public interface IMemberRepository
	{
		Member? GetMemberById(int memberId);

		Task<Member?> GetMemberAsync(int memberId);

		void AddLikedSong(int memberId, int songId);

		void DeleteLikedSong(int memberId, int songId);
	}
}
