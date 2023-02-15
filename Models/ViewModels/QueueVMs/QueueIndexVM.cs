using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.QueueVMs;

namespace api.iSMusic.Models.ViewModels.QueueVMs
{
	public class QueueIndexVM
	{
		public int Id { get; set; }

		public int MemberId { get; set; }

		public int? CurrentSongId { get; set; }

		public int? CurrentSongTime { get; set; }

		public bool IsShuffle { get; set; }

		public bool? IsRepeat { get; set; }

		public int? AlbumId { get; set; }

		public int? ArtistId { get; set; }

		public int? PlaylistId { get; set; }

		public IEnumerable<SongInfoDTO> SongInfos { get; set; } = null!;
	}
}
