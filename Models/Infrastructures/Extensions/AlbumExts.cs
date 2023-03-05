using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.AlbumVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class AlbumExts
{
	public static AlbumIndexVM ToIndexVM(this AlbumIndexDTO source)
		=> new()
		{
			Id = source.Id,
			AlbumName = source.AlbumName,
			AlbumGenreId= source.AlbumGenreId,
			AlbumTypeId= source.AlbumTypeId,
			AlbumCoverPath= source.AlbumCoverPath,
			Released = source.Released,
			MainArtistId= source.MainArtistId,
			MainArtistName= source.MainArtistName,
			MainCreatorId= source.MainCreatorId,
			MainCreatorName= source.MainCreatorName,
			TotalLikes= source.TotalLikes,
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
            MainCreatorId = source.MainCreatorId,
            MainCreatorName = source.MainCreator != null ? source.MainCreator.CreatorName :
                        string.Empty,
            TotalLikes = source.LikedAlbums.Count,
        };

	public static AlbumDetailVM ToDetailVM(this AlbumDetailDTO source)
		=> new()
		{
			Id = source.Id,
			AlbumName = source.AlbumName,
			AlbumCoverPath= source.AlbumCoverPath,
			Released = source.Released,
			MainArtistId= source.MainArtistId,
			AlbumTypeId= source.AlbumTypeId,
			AlbumTypeName= source.AlbumTypeName,
			AlbumGenreId= source.AlbumGenreId,
			AlbumGenreName= source.AlbumGenreName,
			AlbumCompany = source.AlbumCompany,
			AlbumProducer = source.AlbumProducer,
			MainArtistName= source.MainArtistName,
			MainCreatorId= source.MainCreatorId,
			MainCreatorName= source.MainCreatorName,
			Description= source.Description,
			Songs = source.Songs,
		};
    public static AlbumInfoVM ToInfoVM(this Album source)
    => new()
    {
        Id = source.Id,
        AlbumName = source.AlbumName,
        AlbumCoverPath = source.AlbumCoverPath,
        Released = source.Released,
        AlbumTypeId = source.AlbumTypeId,
        AlbumTypeName = source.AlbumType.TypeName,
        AlbumGenreId = source.AlbumGenreId,
        AlbumGenreName = source.AlbumGenre.GenreName,
        AlbumCompany = source.AlbumCompany,
        AlbumProducer = source.AlbumProducer,
        MainArtistName = source.MainArtist.ArtistName,
        Description = source.Description,
    };

    public static AlbumDetailVM ToDetailVM(this Album source)
    => new()
    {
        Id = source.Id,
        AlbumName = source.AlbumName,
        AlbumCoverPath = source.AlbumCoverPath,
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
        Songs = source.Songs.Select(song=>song.ToProductIndexInfoDTO()).OrderBy(dto=>dto.DisplayOrderInAlbum).ToList(),
    };
}

