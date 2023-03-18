namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class CreatorIndexDTO
    {
        public int Id { get; set; }

        public string CreatorName { get; set; } = null!;

        public string? CreatorPicPath { get; set; }
        public string? CreatorAbout { get; set; }
        public string? CreatorCoverPath { get; set; }

        public bool IsLiked { get; set; }

        public int TotalFollows { get; set; }
    }
}
