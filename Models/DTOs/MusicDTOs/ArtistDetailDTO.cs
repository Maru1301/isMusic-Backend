using api.iSMusic.Models.ViewModels.AlbumVMs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
	public class ArtistDetailDTO
	{
		public int Id { get; set; }

		public string ArtistName { get; set; } = null!;

		public string ArtistPicPath { get; set; } = null!;

		public IEnumerable<SongInfoDTO> PopularSongs { get; set; } = null!;

		public IEnumerable<AlbumIndexDTO> PopularAlbums { get; set; } = null!;
	}
}
