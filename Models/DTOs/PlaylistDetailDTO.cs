using api.iSMusic.Models.ViewModels.PlaylistVMs;

namespace api.iSMusic.Models.DTOs
{
	public class PlaylistDetailDTO
	{
		public int Id { get; set; }

		public string ListName { get; set; } = null!;

		public string? PlaylistCoverPath { get; set; }

		public int MemberId { get; set; }

		public bool IsPublic { get; set; }

		public virtual List<PlaylistSongMetadataVM> PlayListSongMetadata { get; set; } = null!;
	}
}
