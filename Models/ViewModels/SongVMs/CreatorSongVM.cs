namespace api.iSMusic.Models.ViewModels.SongVMs
{
    public class CreatorSongVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? GenredName { get; set; }
        public int Duration { get; set; }
        public bool IsInstrumental { get; set; }
        public string Language { get; set; } = null!;
        public bool? IsExplicit { get; set; }
        public DateTime Released { get; set; }
        public string SongWriter { get; set; } = null!;
        public string Lyric { get; set; } = null!;
        public bool Status { get; set; }
        public string? AlbumName { get; set; }
    }
}