using api.iSMusic.Models.DTOs.MusicDTOs;

namespace api.iSMusic.Models.ViewModels.CreatorVMs
{
	public class CreatorDetailVM
	{
		public int Id { get; set; }

		public string CreatorName { get; set; } = null!;

		public string CreatorPicPath { get; set; } = null!;

		public List<SongIndexDTO> PopularSongs { get; set; } = null!;

		public List<AlbumIndexDTO> PopularAlbums { get; set; } = null!;

		public List<PlaylistIndexDTO> IncludedPlaylists { get; set; } = null!;
	}
}
