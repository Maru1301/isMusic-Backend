using api.iSMusic.Models.ViewModels.PlaylistVMs;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class PlaylistDetailDTO
    {
        public int Id { get; set; }

        public string ListName { get; set; } = null!;

        public string? PlaylistCoverPath { get; set; }

        public int MemberId { get; set; }

        public string MemberName { get; set; } = null!;

        public string MemberPicPath { get; set; } = null!;

        public bool IsPublic { get; set; }

        public bool IsLiked { get; set; }

        public bool IsOwner { get; set; }

        public int TotalLikes { get; set; }

        public List<PlaylistSongMetadataDTO> Metadata { get; set; } = null!;
    }
}
