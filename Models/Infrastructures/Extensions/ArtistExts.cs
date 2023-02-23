using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ArtistVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class ArtistExts
{
	public static ArtistInfoVM ToInfoVM(this Artist source)
		=> new()
        {
			ArtistId = source.Id,
			ArtistName = source.ArtistName
		};

	public static ArtistIndexDTO ToIndexDTO(this Artist source)
		=> new()
		{
			Id = source.Id,
			ArtistName = source.ArtistName,
			ArtistPicPath= source.ArtistPicPath,
			TotalFollows = source.ArtistFollows.Count(),
		};

	public static ArtistIndexVM ToIndexVM(this ArtistIndexDTO source)
		=> new()
        {
			Id = source.Id,
			ArtistName = source.ArtistName,
			ArtistPicPath = source.ArtistPicPath,
		};

	public static ArtistDetailVM ToDetailVM(this Artist source)
		=> new()
        {
			Id = source.Id,
			ArtistName = source.ArtistName,
			ArtistPicPath = source.ArtistPicPath,
		};

	public static ArtistDetailVM ToDetailVM(this ArtistDetailDTO source)
		=> new()
        {
			Id = source.Id,
			ArtistName = source.ArtistName,
			ArtistPicPath = source.ArtistPicPath,
			PopularSongs = source.PopularSongs,
			PopularAlbums = source.PopularAlbums,
			IncludedPlaylists = source.IncludedPlaylists,
		};

	public static ArtistAboutVM ToAboutVM(this ArtistAboutDTO source)
		=> new()
		{
			Id = source.Id,
			ArtistName = source.ArtistName,
			About = source.About,
			Followers = source.Followers,
			MonthlyPlayedTimes = source.MonthlyPlayedTimes,
		};

}
