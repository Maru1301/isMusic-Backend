using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.ViewModels.PlaylistVMs
{
	public class PlaylistDetailVM
	{
		public int Id { get; set; }

		public string ListName { get; set; } = null!;

		public string? PlaylistCoverPath { get; set; }

		public string MemberName { get; set; } = null!;

		public string MemberPicPath { get; set; } = null!;

        public bool IsPublic { get; set; }

        public bool IsLiked { get; set; }

        public bool IsOwner { get; set; }

        public int TotalLikes { get; set; }

        public List<PlaylistSongMetadataVM> Metadata { get; set; } = null!;
	}
}
