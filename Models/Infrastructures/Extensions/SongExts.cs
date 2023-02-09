using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.SongVMs;

namespace API_practice.Models.Infrastructures.Extensions;

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
            AlbumName = source.Album.AlbumName,
        };
}