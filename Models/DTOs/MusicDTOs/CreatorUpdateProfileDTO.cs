namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class CreatorUpdateProfileDTO
    {
		public int Id { get; set; }
		public string? CreatorName { get; set; }
		public string? CreatorAbout { get; set; }
		public string? CreatorPicPath { get; set; }
		public string? CreatorCoverPath { get; set; }
		public IFormFile? Pic { get; set; }
		public IFormFile? Cover { get; set; }
	}
}