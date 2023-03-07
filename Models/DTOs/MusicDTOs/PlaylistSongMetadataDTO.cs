namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class PlaylistSongMetadataDTO
    {
        public int Id { get; set; }

        public int PlayListId { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime AddedTime { get; set; }

        public SongInfoDTO Song { get; set; } = null!;
    }
}
