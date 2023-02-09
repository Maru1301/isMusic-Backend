using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.AlbumVMs;
using API_practice.Models.ViewModels.SongVMs;

namespace API_practice.Models.ViewModels.ArtistVMs
{
	public class ArtistDetailVM
	{
		public int Id { get; set; }

		public string ArtistName { get; set; } = null!;

		public string ArtistPicPath { get; set; } = null!;

		public IEnumerable<SongInfoVM> PopularSongs { get; set; } = null!;

		public IEnumerable<AlbumIndexVM> PopularAlbums { get; set; } = null!;
	}
}
