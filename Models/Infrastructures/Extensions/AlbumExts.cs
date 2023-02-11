using api.iSMusic.Models.DTOs.MusicDTOs;
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
			TotalLikes= source.TotalLikes,
		};
}

