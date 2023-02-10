using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.Services.Interfaces
{
	public interface IMemberRepository
	{
		Member? GetMember(int memberId);

		Task<Member?> GetMemberAsync(int memberId);
	}
}
