using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ArtistVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class ArtistExts
{
	private static readonly string webUrl = "https://localhost:44373/Uploads/Covers/";

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
			ArtistPicPath = webUrl + source.ArtistPicPath,
		};

	public static ArtistDetailVM ToDetailVM(this ArtistDetailDTO source)
		=> new()
        {
			Id = source.Id,
			ArtistName = source.ArtistName,
			ArtistPicPath = webUrl + source.ArtistPicPath,
			About = source.About,
			IsLiked = source.IsLiked,
			TotalFollows = source.TotalFollowed,
			PopularSongs = source.PopularSongs.Select(dto => dto.ToIndexVM()).ToList(),
			PopularAlbums = source.PopularAlbums.Select(dto => dto.ToIndexVM()).ToList(),
			IncludedPlaylists = source.IncludedPlaylists.Select(dto => dto.ToIndexVM()).ToList(),
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
