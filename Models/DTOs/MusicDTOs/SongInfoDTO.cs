using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class SongInfoDTO
    {
        public int Id { get; set; }

        public string SongName { get; set; } = null!;

        public int Duration { get; set; }

        public bool? IsExplicit { get; set; }

        public DateTime Released { get; set; }

        public string SongCoverPath { get; set; } = null!;

        public string SongPath { get; set; } = null!;

        public bool Status { get; set; }

        public int? AlbumId { get; set; }

        public string AlbumName { get; set; } = null!;

        public bool IsLiked { get; set; }

        public bool FromList { get; set; }

        public int PlayedTimes { get; set; }

        public int? DisplayOrderInAlbum { get; set; }
        
        public List<ArtistInfoVM> Artists { get; set; } = null!;

        public List<CreatorInfoVM> Creators { get; set; } = null!;
    }
}
