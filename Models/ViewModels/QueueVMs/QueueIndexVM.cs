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

		public virtual Song? CurrentSong { get; set; }

		public IEnumerable<int> SongIds { get; set; } = null!;
	}
}

public static class QueueExts
{
	public static QueueIndexVM ToIndexVM(this Queue source)
		=> new QueueIndexVM
		{
			Id= source.Id,
			MemberId= source.MemberId,
			CurrentSongId= source.CurrentSongId,
			CurrentSongTime= source.CurrentSongTime,
			IsShuffle= source.IsShuffle,
			IsRepeat= source.IsRepeat,
			SongIds = source.QueueSongs.OrderBy(qs => qs.DisplayOrder).Select(qs => qs.Id),
		};
}
