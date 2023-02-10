using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.PlaylistVMs;

namespace api.iSMusic.Models.ViewModels.PlaylistVMs
{
	public class PlaylistCreateVM
	{
		public string ListName { get; set; } = null!;

		public int MemberId { get; set; }
	}
}
