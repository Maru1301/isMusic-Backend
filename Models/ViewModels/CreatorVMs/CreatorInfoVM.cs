using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.CreatorVMs;

namespace API_practice.Models.ViewModels.CreatorVMs
{
	public class CreatorInfoVM
	{
		public int CreatorId { get; set; }

		public string CreatorName { get; set; } = null!;
	}
}

public static class CreatorExts
{
	public static CreatorInfoVM ToInfoVM(this Creator source)
		=> new CreatorInfoVM
		{
			CreatorId = source.Id,
			CreatorName = source.CreatorName
		};
}