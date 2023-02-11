using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class SongIndexDTO
    {
        public int Id { get; set; }

        public string SongName { get; set; } = null!;

        public string GenreName { get; set; } = null!;

        public bool? IsExplicit { get; set; }

        public string SongCoverPath { get; set; } = null!;

        public string SongPath { get; set; } = null!;

        public int AlbumId { get; set; }

        public int PlayedTimes { get; set; }

        public IEnumerable<ArtistInfoVM> Artistlist { get; set; } = new List<ArtistInfoVM>();

        public IEnumerable<CreatorInfoVM> Creatorlist { get; set; } = new List<CreatorInfoVM>();
    }
}
