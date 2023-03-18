using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.ViewModels.PlaylistVMs
{
	public class PlaylistSongMetadataVM
	{
		public int Id { get; set; }

		public int PlayListId { get; set; }

		public int SongId { get; set; }

		public int DisplayOrder { get; set; }

		public DateTime AddedTime { get; set; }

		public SongInfoVM Song { get; set; } = null!;
	}
}
