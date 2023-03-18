using api.iSMusic.Controllers;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class SongExts
{
    private static readonly string webPicUrl = "https://localhost:44373/Uploads/Covers/";

    private static readonly string webSongUrl = "https://localhost:44373/Uploads/Songs/";

    public static SongInfoVM ToInfoVM(this SongInfoDTO source)
        => new()
        {
            Id = source.Id,
            SongName = source.SongName,
            Duration = source.Duration,
            IsExplicit = source.IsExplicit,
            IsLiked = source.IsLiked,
            Released = source.Released,
            SongCoverPath = webPicUrl + source.SongCoverPath,
            SongPath = webSongUrl + source.SongPath,
            Status = source.Status,
            AlbumId = source.AlbumId,
            FromList = source.FromList,
            AlbumName = source.AlbumName,
            DisplayOrderInAlbum = source.DisplayOrderInAlbum,
            PlayedTimes = source.PlayedTimes,
            Artists = source.Artists,
            Creators = source.Creators,
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
            FromList = true,
            AlbumName = source.Album != null ? source.Album.AlbumName: string.Empty,
            Artists = source.SongArtistMetadata.Select(metadata => metadata.Artist.ToInfoVM()).ToList(),
            Creators = source.SongCreatorMetadata.Select(metadata => metadata.Creator.ToInfoVM()).ToList(),
            DisplayOrderInAlbum = source.DisplayOrderInAlbum,
            PlayedTimes = source.SongPlayedRecords.Count,
		};

    public static SongInfoDTO ToInfoDTONotFromList(this Song source)
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
            FromList = false,
            AlbumName = source.Album != null ? source.Album.AlbumName : string.Empty,
            Artists = source.SongArtistMetadata.Select(metadata => metadata.Artist.ToInfoVM()).ToList(),
            Creators = source.SongCreatorMetadata.Select(metadata => metadata.Creator.ToInfoVM()).ToList(),
            DisplayOrderInAlbum = source.DisplayOrderInAlbum,
            PlayedTimes = source.SongPlayedRecords.Count,
        };

    public static SongIndexVM ToIndexVM(this SongIndexDTO source)
        => new()
		{
            Id = source.Id,
            SongName = source.SongName,
            GenreName = source.GenreName,
            Duration = source.Duration,
            IsExplicit = source.IsExplicit,
            SongCoverPath = webPicUrl + source.SongCoverPath,
            SongPath = webSongUrl + source.SongPath,
            AlbumId = source.AlbumId,
            PlayedTimes = source.PlayedTimes,
            Artistlist = source.Artistlist,
            Creatorlist = source.Creatorlist,
            IsLiked = source.IsLiked,
        };

    public static SongInfoVM ToProductIndexInfoVM(this Song source)
        => new()
        {
            Id = source.Id,
            SongName = source.SongName,
            DisplayOrderInAlbum=source.DisplayOrderInAlbum,
        };
}