namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class SongDTO
    {
        public int Id { get; set; }
        public string? SongName { get; set; }
        public int GenreId { get; set; }
        public string? GenreName { get; set; }
        public int Duration { get; set; }
        public bool IsInstrumental { get; set; }
        public string? Language { get; set; }
        public bool? IsExplicit { get; set; }
        public DateTime Released { get; set; }
        public string? SongWriter { get; set; }
        public string? Lyric { get; set; }
        public string? SongCoverPath { get; set; }
        public string? SongPath { get; set; }
        public bool Status { get; set; }
        public int? AlbumId { get; set; }
        public string? AlbumName { get; set; }
        public string? Cover { get; set; }
    
    }
}