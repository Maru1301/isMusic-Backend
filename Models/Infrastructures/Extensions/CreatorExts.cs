using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions
{
    public static class CreatorExts
	{
		public static CreatorIndexVM ToIndexVM(this CreatorIndexDTO source)
			=> new()
			{
				Id = source.Id,
				CreatorName = source.CreatorName,
				CreatorPicPath = source.CreatorPicPath,
			};

		public static CreatorDetailVM ToDetailVM(this CreatorDetailDTO source)
			=> new()
			{
				Id = source.Id,
				CreatorName = source.CreatorName,
				CreatorPicPath = source.CreatorPicPath,
				PopularSongs = source.PopularSongs,
				PopularAlbums = source.PopularAlbums,
				IncludedPlaylists = source.IncludedPlaylists,
			};
	}
}
