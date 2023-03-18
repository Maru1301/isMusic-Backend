using api.iSMusic.Models.ViewModels.AlbumVMs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
	public class ArtistDetailDTO
	{
		public int Id { get; set; }

		public string ArtistName { get; set; } = null!;

		public string ArtistPicPath { get; set; } = null!;

        public string About { get; set; } = null!;

        public bool IsLiked { get; set; }

        public int TotalFollowed { get; set; }

        public List<SongIndexDTO> PopularSongs { get; set; } = null!;

		public List<AlbumIndexDTO> PopularAlbums { get; set; } = null!;

		public List<PlaylistIndexDTO> IncludedPlaylists { get; set; } = null!;
	}
}
