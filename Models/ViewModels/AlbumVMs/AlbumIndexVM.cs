using API_practice.Models.EFModels;

namespace API_practice.Models.ViewModels.AlbumVMs
{
	public class AlbumIndexVM
	{
		public int Id { get; set; }

		public string AlbumName { get; set; } = null!;

		public string AlbumCoverPath { get; set; } = null!;

		public int AlbumTypeId { get; set; }

		public int AlbumGenreId { get; set; }

		public DateTime Released { get; set; }

		public int MainArtistId { get; set; }
	}
}
