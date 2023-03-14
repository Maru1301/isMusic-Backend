namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class CreatorDTO
    {
        public int Id { get; set; }
        public string? CreatorName { get; set; }
        public string? CreatorAbout { get; set; }
        public string? CreatorPicPath { get; set; }
        public string? CreatorCoverPath { get; set; }
        public int TotalFollows { get; set; }
        public string? CreatorPic { get; set; }
        public string? CreatorCover { get; set; }
    }
}