using api.iSMusic.Models.DTOs;

namespace api.iSMusic.Models.Services.Interfaces
{
	public interface IQueueRepository
	{
		Task<QueueIndexDTO?> GetQueueByMemberIdAsync(int memberId);
	}
}
