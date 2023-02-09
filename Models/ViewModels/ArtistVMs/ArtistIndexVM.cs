using API_practice.Models.EFModels;

namespace API_practice.Models.ViewModels.ArtistVMs
{
	public class ArtistIndexVM
	{
		public int Id { get; set; }

		public string ArtistName { get; set; } = null!;

		public string ArtistPicPath { get; set; } = null!;
	}
}
