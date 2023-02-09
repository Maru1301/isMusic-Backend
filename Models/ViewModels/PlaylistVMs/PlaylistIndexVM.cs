using System.Runtime.CompilerServices;

namespace API_practice.Models.ViewModels.PlaylistVMs
{
	public class PlaylistIndexVM
	{
		public int Id { get; set; }

		public string ListName { get; set; } = null!;

		public string? PlaylistCoverPath { get; set; }

		public string MemberAccount { get; set; } = null!;
	}
}
