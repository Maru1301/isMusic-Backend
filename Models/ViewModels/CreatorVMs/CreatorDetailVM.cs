using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.ViewModels.AlbumVMs;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.ViewModels.CreatorVMs
{
	public class CreatorDetailVM
	{
		public int Id { get; set; }

		public string CreatorName { get; set; } = null!;

		public string CreatorPicPath { get; set; } = null!;

		public string About { get; set; } = null!;

        public bool IsLiked { get; set; }

        public int TotalFollows { get; set; }

        public List<SongIndexVM> PopularSongs { get; set; } = null!;

		public List<AlbumIndexVM> PopularAlbums { get; set; } = null!;

		public List<PlaylistIndexVM> IncludedPlaylists { get; set; } = null!;
	}
}
