using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.SongVMs;

namespace API_practice.Models.ViewModels.PlaylistVMs
{
	public class PlaylistSongMetadataVM
	{
		public int Id { get; set; }

		public int PlayListId { get; set; }

		public int SongId { get; set; }

		public int DisplayOrder { get; set; }

		public DateTime AddedTime { get; set; }

		public virtual SongInfoVM Song { get; set; } = null!;
	}
}
