using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;
using System.Reflection.Metadata.Ecma335;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class SongIndexDTO
    {
        public int Id { get; set; }

        public string SongName { get; set; } = string.Empty;

		public string GenreName { get; set; } = string.Empty;

        public int Duration { get; set; }

        public bool? IsExplicit { get; set; }

        public bool IsLiked { get; set; }

        public string SongCoverPath { get; set; } = string.Empty;

		public string SongPath { get; set; } = string.Empty;

		public string SongWriter { get; set; } = string.Empty;

		public int AlbumId { get; set; }

        public int PlayedTimes { get; set; }

        public IEnumerable<ArtistInfoVM> Artistlist { get; set; } = new List<ArtistInfoVM>();

        public IEnumerable<CreatorInfoVM> Creatorlist { get; set; } = new List<CreatorInfoVM>();
    }
}
