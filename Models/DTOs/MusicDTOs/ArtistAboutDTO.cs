namespace api.iSMusic.Models.DTOs.MusicDTOs
{
	public class ArtistAboutDTO
	{
		public int Id { get; set; }

		public string? ArtistName { get; set; }

		public string? About { get; set; }

		public int Followers { get; set; }

		public int MonthlyPlayedTimes { get; set; }
	}
}
