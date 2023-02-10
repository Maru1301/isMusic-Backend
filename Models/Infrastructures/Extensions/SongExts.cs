using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class SongExts
{
    public static SongInfoVM ToInfoVM(this SongInfoDTO source)
        => new SongInfoVM
        {
            Id = source.Id,
            SongName = source.SongName,
            Duration = source.Duration,
            IsExplicit = source.IsExplicit,
            Released = source.Released,
            SongCoverPath = source.SongCoverPath,
            SongPath = source.SongPath,
            Status = source.Status,
            AlbumId = source.AlbumId,
            AlbumName = source.AlbumName,
        };

    public static SongIndexVM ToIndexVM(this SongIndexDTO source)
        => new()
		{
            Id = source.Id,
            SongName = source.SongName,
            GenreName = source.GenreName,
            IsExplicit = source.IsExplicit,
            SongCoverPath = source.SongCoverPath,
            SongPath = source.SongPath,
            AlbumId = source.AlbumId,
            PlayedTimes = source.PlayedTimes,
            Artistlist = source.Artistlist,
            Creatorlist = source.Creatorlist,
        };
}