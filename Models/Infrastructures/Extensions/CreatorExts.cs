using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions
{
    public static class CreatorExts
	{
        private static readonly string webUrl = "https://localhost:44373/Uploads/Covers/";

		public static CreatorIndexVM ToIndexVM(this CreatorIndexDTO source)
			=> new()
			{
				Id = source.Id,
				CreatorName = source.CreatorName,
				CreatorPicPath = webUrl + source.CreatorPicPath,
			};

		public static CreatorDetailVM ToDetailVM(this CreatorDetailDTO source)
			=> new()
			{
				Id = source.Id,
				CreatorName = source.CreatorName,
				CreatorPicPath = webUrl + source.CreatorPicPath,
				About = source.About,
				IsLiked = source.IsLiked,
				TotalFollows = source.TotalFollowed,
				PopularSongs = source.PopularSongs.Select(song => song.ToIndexVM()).ToList(),
				PopularAlbums = source.PopularAlbums.Select(dto => dto.ToIndexVM()).ToList(),
				IncludedPlaylists = source.IncludedPlaylists.Select(dto => dto.ToIndexVM()).ToList(),
			};
	}
}
