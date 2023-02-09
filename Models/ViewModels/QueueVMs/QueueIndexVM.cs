using API_practice.Models.EFModels;

namespace API_practice.Models.ViewModels.QueueVMs
{
	public class QueueIndexVM
	{
		public int Id { get; set; }

		public string MemberAccount { get; set; } = null!;

		public int? CurrentSongId { get; set; }

		public int? CurrentSongTime { get; set; }

		public bool IsShuffle { get; set; }

		public bool? IsRepeat { get; set; }

		public virtual Song? CurrentSong { get; set; }

		public virtual ICollection<QueueSong> QueueSongs { get; } = new List<QueueSong>();
	}
}
