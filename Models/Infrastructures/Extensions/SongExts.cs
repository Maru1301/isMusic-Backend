using api.iSMusic.Models.DTOs.MusicDTOs;
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

	public static SongInfoDTO ToInfoDTO(this Song source)
		=> new()
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
            AlbumName = source.Album != null ? source.Album.AlbumName: string.Empty,
            Artists = source.SongArtistMetadata.Select(metadata => metadata.Artist.ToInfoVM()).ToList(),
            Creators = source.SongCreatorMetadata.Select(metadata => metadata.Creator.ToInfoVM()).ToList(),
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

    public static SongInfoDTO ToProductIndexInfoDTO(this Song source)
    => new SongInfoDTO
    {
        Id = source.Id,
        SongName = source.SongName,
        DisplayOrderInAlbum=source.DisplayOrderInAlbum,

    };
}