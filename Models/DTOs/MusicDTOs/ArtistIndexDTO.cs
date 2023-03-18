namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class ArtistIndexDTO
    {
        public int Id { get; set; }

        public string ArtistName { get; set; } = null!;

        public string ArtistPicPath { get; set; } = null!;

        public string About { get;set; } = null!;

        public int TotalFollows { get; set; }

        public bool IsLiked { get; set; }
    }
}
