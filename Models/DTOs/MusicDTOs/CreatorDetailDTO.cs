namespace api.iSMusic.Models.DTOs.MusicDTOs
{
	public class CreatorDetailDTO
	{
		public int Id { get; set; }

		public string CreatorName { get; set; } = null!;

		public string CreatorPicPath { get; set; } = null!;

        public string About { get; set; } = null!;

        public bool IsLiked { get; set; }

        public int TotalFollowed { get; set; }

        public List<SongIndexDTO> PopularSongs { get; set; } = null!;

		public List<AlbumIndexDTO> PopularAlbums { get; set; } = null!;

		public List<PlaylistIndexDTO> IncludedPlaylists { get; set; } = null!;
	}
}
