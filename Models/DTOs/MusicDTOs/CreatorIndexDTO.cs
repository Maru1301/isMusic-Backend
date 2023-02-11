namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class CreatorIndexDTO
    {
        public int Id { get; set; }

        public string CreatorName { get; set; } = null!;

        public string CreatorPicPath { get; set; } = null!;

        public int TotalFollows { get; set; }
    }
}
