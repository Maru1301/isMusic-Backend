using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using static api.iSMusic.Controllers.MembersController;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface ICreatorRepository
	{
        IEnumerable<CreatorIndexDTO> GetRecommended();

        IEnumerable<CreatorIndexDTO> GetLikedCreators(int memberId, LikedQuery body);

		CreatorIndexDTO? GetCreatorById(int creatorId);

        Creator? GetCreatorByIdForCheck(int creatorId);

        IEnumerable<CreatorIndexDTO> GetCreatorsByName(string name, int rowNumber);

        void CreateCreator(int memberId);

    }
}
