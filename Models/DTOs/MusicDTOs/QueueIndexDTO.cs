using api.iSMusic.Models.EFModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class QueueIndexDTO
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int? CurrentSongOrder { get; set; }

        public int? CurrentSongTime { get; set; }

        public bool IsShuffle { get; set; }

        public bool? IsRepeat { get; set; }

        public bool InList { get; set; }

        public int? AlbumId { get; set; }

        public int? ArtistId { get; set; }
        
        public int? PlaylistId { get; set; }

        public List<SongInfoDTO> SongInfos { get; set; } = null!;

        public List<QueueSong> QueueSongs { get; set; } = null!;
    }
}
