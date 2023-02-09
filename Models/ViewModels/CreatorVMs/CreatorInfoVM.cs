using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.CreatorVMs;

namespace api.iSMusic.Models.ViewModels.CreatorVMs
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