namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class ArtistIndexDTO
    {
        public int Id { get; set; }

        public string ArtistName { get; set; } = null!;

        public string ArtistPicPath { get; set; } = null!;

        public int Follows { get; set; }
    }
}
