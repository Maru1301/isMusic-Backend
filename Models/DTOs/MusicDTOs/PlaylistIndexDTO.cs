namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class PlaylistIndexDTO
    {
        public int Id { get; set; }

        public string ListName { get; set; } = null!;

        public string? PlaylistCoverPath { get; set; }

        public int MemberId { get; set; }

        public int TotalLikes { get; set; }
    }
}
