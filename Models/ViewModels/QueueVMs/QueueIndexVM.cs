using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.QueueVMs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.ViewModels.QueueVMs
{
	public class QueueIndexVM
	{
		public int Id { get; set; }

		public int MemberId { get; set; }

		public int? CurrentSongOrder { get; set; }

		public int? CurrentSongTime { get; set; }

		public bool IsShuffle { get; set; }

		public bool? IsRepeat { get; set; }

		public int? AlbumId { get; set; }

		public int? ArtistId { get; set; }

		public int? PlaylistId { get; set; }

		public IEnumerable<SongInfoVM> SongInfos { get; set; } = null!;
	}
}
