namespace api.iSMusic.Models.ViewModels.SongVMs
{
	public class SongCreditsVM
	{
		public string SongName { get; set; } = null!;

		public List<string> Artists { get; set; } = null!;

		public string SongWriter { get; set; } = null!;
	}
}
