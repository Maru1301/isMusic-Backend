using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.PlaylistVMs;

namespace API_practice.Models.ViewModels.PlaylistVMs
{
	public class PlaylistCreateVM
	{
		public string ListName { get; set; } = null!;

		public string MemberAccount { get; set; } = null!;
	}
}
