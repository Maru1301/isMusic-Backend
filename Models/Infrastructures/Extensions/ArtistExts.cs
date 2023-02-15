using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ArtistVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class ArtistExts
{
	public static ArtistInfoVM ToInfoVM(this Artist source)
		=> new ArtistInfoVM
		{
			ArtistId = source.Id,
			ArtistName = source.ArtistName
		};

	public static ArtistIndexVM ToIndexVM(this ArtistIndexDTO source)
		=> new ArtistIndexVM
		{
			Id = source.Id,
			ArtistName = source.ArtistName,
			ArtistPicPath = source.ArtistPicPath,
		};

	public static ArtistDetailVM ToDetailVM(this Artist source)
		=> new ArtistDetailVM
		{
			Id = source.Id,
			ArtistName = source.ArtistName,
			ArtistPicPath = source.ArtistPicPath,
		};
}
