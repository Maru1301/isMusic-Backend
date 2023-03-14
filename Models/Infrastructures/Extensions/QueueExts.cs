using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.ViewModels.QueueVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions
{
    public static class QueueExts
	{
		public static QueueIndexVM ToIndexVM(this QueueIndexDTO source)
			=> new()
			{
				Id= source.Id,
				CurrentSongOrder = source.CurrentSongOrder,
				CurrentSongTime= source.CurrentSongTime,
				IsShuffle= source.IsShuffle,
				IsRepeat= source.IsRepeat,
				MemberId= source.MemberId,
				SongInfos = source.SongInfos.Select(dto => dto.ToInfoVM()),
				AlbumId= source.AlbumId,
				ArtistId= source.ArtistId,
				PlaylistId= source.PlaylistId,
			};
	}
}
