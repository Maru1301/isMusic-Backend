using api.iSMusic.Models.DTOs.MusicDTOs;

namespace api.iSMusic.Models.Services.Interfaces
{
    public interface ICreatorRepository
	{
		IEnumerable<CreatorIndexDTO> GetCreatorsByName(string name, int skipRows, int takeRows);
	}
}
