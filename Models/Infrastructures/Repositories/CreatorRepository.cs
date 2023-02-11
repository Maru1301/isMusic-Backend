using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class CreatorRepository: IRepository, ICreatorRepository
	{
		private readonly AppDbContext _db;

		public CreatorRepository(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<CreatorIndexDTO> GetCreatorsByName(string name, int skipRows, int takeRows)
		{
			return _db.Creators
				.Where(creator => creator.CreatorName.Contains(name))
				.Select(creator => new CreatorIndexDTO
				{
					Id = creator.Id,
					CreatorName = creator.CreatorName,
					CreatorPicPath = creator.CreatorPicPath,
					TotalFollows = creator.CreatorFollows.Count,
				})
				.OrderBy(dto => dto.TotalFollows)
				.Skip(skipRows)
				.Take(takeRows)
				.ToList();
		}
	}
}
