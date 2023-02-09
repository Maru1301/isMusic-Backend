namespace API_practice.Models.ViewModels.PlaylistVMs
{
	public class PlaylistDetailVM
	{
		public int Id { get; set; }

		public string ListName { get; set; } = null!;

		public string? PlaylistCoverPath { get; set; }

		public string MemberAccount { get; set; } = null!;

		public bool IsPublic { get; set; }

		public virtual List<PlaylistSongMetadataVM> PlayListSongMetadata { get; set; } = null!;
	}
}
