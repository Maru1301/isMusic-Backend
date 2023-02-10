using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.ViewModels.QueueVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions
{
	public static class QueueExts
	{
		public static QueueIndexVM ToIndexVM(this QueueIndexDTO source)
			=> new()
			{
				Id= source.Id,
				SongIds= source.SongIds,
				CurrentSong= source.CurrentSong,
				CurrentSongId= source.CurrentSongId,
				CurrentSongTime= source.CurrentSongTime,
				IsShuffle= source.IsShuffle,
				IsRepeat= source.IsRepeat,
				MemberId= source.MemberId,
			};

	}
}
