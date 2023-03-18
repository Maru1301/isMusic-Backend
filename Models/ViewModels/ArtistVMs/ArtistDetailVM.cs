using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.AlbumVMs;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.ViewModels.ArtistVMs
{
	public class ArtistDetailVM
	{
		public int Id { get; set; }

		public string ArtistName { get; set; } = null!;

		public string ArtistPicPath { get; set; } = null!;

        public string About { get; set; } = null!;

        public bool IsLiked { get; set; }

        public int TotalFollows { get; set; }

        public List<SongIndexVM> PopularSongs { get; set; } = new();

		public List<AlbumIndexVM> PopularAlbums { get; set; } = new();

		public List<PlaylistIndexVM> IncludedPlaylists { get; set; } = new();
	}
}
