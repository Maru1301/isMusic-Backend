namespace api.iSMusic.Models.DTOs.MusicDTOs
{
	public class SongCreditsDTO
	{
		public string SongName { get; set; } = null!;

		public List<string> Artists { get; set; } = null!;

		public string SongWriter { get; set; } = null!;
	}
}
