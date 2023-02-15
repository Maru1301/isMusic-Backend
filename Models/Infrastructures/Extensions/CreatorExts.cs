using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions
{
    public static class CreatorExts
	{
		public static CreatorIndexVM ToIndexVM(this CreatorIndexDTO source)
		=> new CreatorIndexVM
		{
			Id = source.Id,
			CreatorName = source.CreatorName,
			CreatorPicPath = source.CreatorPicPath,
		};
	}
}
