using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ArtistVMs;

public static class ArtistExts
{
	public static ArtistInfoVM ToInfoVM(this Artist source)
		=> new ArtistInfoVM
		{
			ArtistId = source.Id,
			ArtistName = source.ArtistName
		};

	public static ArtistIndexVM ToIndexVM(this Artist source)
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
