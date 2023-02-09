using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class SongExts
{
    public static SongInfoVM ToInfoVM(this Song source)
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
            AlbumName = source.Album != null? source.Album.AlbumName: "",
        };
}