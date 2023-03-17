using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class PlaylistIndexDTO
    {
        public int Id { get; set; }

        public string ListName { get; set; } = null!;

        public string? PlaylistCoverPath { get; set; }

        public int MemberId { get; set; }

        public string? OwnerName { get; set; }

        public int TotalLikes { get; set; }

        public bool IsLiked { get; set; }

        public bool IsPublic { get; set; }

        public IEnumerable<PlaylistSongMetadatum> PlaylistSongMetadata { get; set; } = null!;
	}
}
