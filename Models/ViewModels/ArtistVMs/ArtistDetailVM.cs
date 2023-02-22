using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.AlbumVMs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.ViewModels.ArtistVMs
{
	public class ArtistDetailVM
	{
		public int Id { get; set; }

		public string ArtistName { get; set; } = null!;

		public string ArtistPicPath { get; set; } = null!;

		public List<SongIndexDTO> PopularSongs { get; set; } = new();

		public List<AlbumIndexDTO> PopularAlbums { get; set; } = new();

		public List<PlaylistIndexDTO> IncludedPlaylists { get; set; } = new();
	}
}
