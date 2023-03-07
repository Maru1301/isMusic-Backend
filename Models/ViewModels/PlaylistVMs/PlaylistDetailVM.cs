using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.ViewModels.PlaylistVMs
{
	public class PlaylistDetailVM
	{
		public int Id { get; set; }

		public string ListName { get; set; } = null!;

		public string? PlaylistCoverPath { get; set; }

		public int MemberId { get; set; }

		public bool IsPublic { get; set; }

		public List<PlaylistSongMetadataVM> Metadata { get; set; } = null!;
	}
}
