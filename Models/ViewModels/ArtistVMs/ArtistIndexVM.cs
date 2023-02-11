using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.ViewModels.ArtistVMs
{
	public class ArtistIndexVM
	{
		public int Id { get; set; }

		public string ArtistName { get; set; } = null!;

		public string ArtistPicPath { get; set; } = null!;

		public int Follows { get; set; }
	}
}
