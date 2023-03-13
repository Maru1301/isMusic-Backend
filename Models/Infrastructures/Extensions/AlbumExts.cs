using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.AlbumVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class AlbumExts
{
    private static readonly string webPicUrl = "https://localhost:44373/Uploads/Covers/";

    public static AlbumIndexVM ToIndexVM(this AlbumIndexDTO source)
		=> new()
		{
			Id = source.Id,
			AlbumName = source.AlbumName,
			AlbumGenreId= source.AlbumGenreId,
			AlbumTypeId= source.AlbumTypeId,
			AlbumCoverPath= webPicUrl + source.AlbumCoverPath,
			Released = source.Released,
			MainArtistId= source.MainArtistId,
			MainArtistName= source.MainArtistName,
            MainArtistPicPath = source.MainArtistPicPath,
			MainCreatorId= source.MainCreatorId,
			MainCreatorName= source.MainCreatorName,
            MainCreatorPicPath = source.MainCreatorPicPath,
			TotalLikes= source.TotalLikes,
            IsLiked= source.IsLiked,
		};

	public static AlbumIndexDTO ToIndexDTO(this Album source)
		=> new()
		{
            Id = source.Id,
            AlbumCoverPath = source.AlbumCoverPath,
            AlbumName = source.AlbumName,
            AlbumGenreId = source.AlbumGenreId,
            AlbumGenreName = source.AlbumGenre.GenreName,
            AlbumTypeId = source.AlbumTypeId,
            AlbumTypeName = source.AlbumType.TypeName,
            Released = source.Released,
            MainArtistId = source.MainArtistId,
            MainArtistName = source.MainArtist != null ?
                        source.MainArtist.ArtistName :
                        string.Empty,
            MainArtistPicPath = source.MainArtist != null ?
                        source.MainArtist.ArtistPicPath :
                        string.Empty, 
            MainCreatorId = source.MainCreatorId,
            MainCreatorName = source.MainCreator != null ? source.MainCreator.CreatorName :
                        string.Empty,
            MainCreatorPicPath = source.MainCreator != null?
                source.MainCreator.CreatorPicPath : 
            string.Empty,
            TotalLikes = source.LikedAlbums.Count,
        };

	public static AlbumDetailVM ToDetailVM(this AlbumDetailDTO source)
		=> new()
		{
			Id = source.Id,
			AlbumName = source.AlbumName,
			AlbumCoverPath= webPicUrl + source.AlbumCoverPath,
			Released = source.Released,
			MainArtistId= source.MainArtistId,
			AlbumTypeId= source.AlbumTypeId,
			AlbumTypeName= source.AlbumTypeName,
            IsLiked = source.IsLiked,
			AlbumGenreId= source.AlbumGenreId,
			AlbumGenreName= source.AlbumGenreName,
			AlbumCompany = source.AlbumCompany,
			AlbumProducer = source.AlbumProducer,
			MainArtistName= source.MainArtistName,
            MainArtistPicPath= string.IsNullOrEmpty(source.MainArtistPicPath)? "": webPicUrl + source.MainArtistPicPath,
			MainCreatorId= source.MainCreatorId,
			MainCreatorName= source.MainCreatorName,
            MainCreatorPicPath= string.IsNullOrEmpty(source.MainCreatorPicPath) ? "" : webPicUrl + source.MainCreatorPicPath,
			Description= source.Description,
			Songs = source.Songs.Select(song => song.ToInfoVM()).OrderBy(dto => dto.DisplayOrderInAlbum).ToList(),
		};
    public static AlbumInfoVM ToInfoVM(this Album source)
    => new()
    {
        Id = source.Id,
        AlbumName = source.AlbumName,
        AlbumCoverPath = webPicUrl + source.AlbumCoverPath,
        Released = source.Released,
        AlbumTypeId = source.AlbumTypeId,
        AlbumTypeName = source.AlbumType.TypeName,
        AlbumGenreId = source.AlbumGenreId,
        AlbumGenreName = source.AlbumGenre.GenreName,
        AlbumCompany = source.AlbumCompany,
        AlbumProducer = source.AlbumProducer,
        MainArtistName = source.MainArtist!.ArtistName,
        Description = source.Description,
    };

    public static AlbumDetailVM ToDetailVM(this Album source)
    => new()
    {
        Id = source.Id,
        AlbumName = source.AlbumName,
        AlbumCoverPath = webPicUrl + source.AlbumCoverPath,
        Released = source.Released,
        MainArtistId = source.MainArtistId,
        AlbumTypeId = source.AlbumTypeId,
        AlbumTypeName = source.AlbumType.TypeName,
        AlbumGenreId = source.AlbumGenreId,
        AlbumGenreName = source.AlbumGenre.GenreName,
        AlbumCompany = source.AlbumCompany,
        AlbumProducer = source.AlbumProducer,
        MainArtistName = source.MainArtist!.ArtistName,
        Description = source.Description,
        Songs = source.Songs.Select(song=>song.ToProductIndexInfoVM()).OrderBy(dto=>dto.DisplayOrderInAlbum).ToList(),
    };
}

