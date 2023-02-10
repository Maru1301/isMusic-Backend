using api.iSMusic.Models.EFModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.DTOs
{
	public class QueueIndexDTO
	{
		public int Id { get; set; }

		public int MemberId { get; set; }

		public int? CurrentSongId { get; set; }

		public int? CurrentSongTime { get; set; }

		public bool IsShuffle { get; set; }

		public bool? IsRepeat { get; set; }

		public virtual Song? CurrentSong { get; set; }

		public IEnumerable<int> SongIds { get; set; } = null!;

	}
}
