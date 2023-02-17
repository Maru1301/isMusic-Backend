using api.iSMusic.Controllers;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class CreatorRepository: IRepository, ICreatorRepository
	{
		private readonly AppDbContext _db;

		private readonly int skipNumber = 5;

		private readonly int takeNumber = 5;

		public CreatorRepository(AppDbContext db)
		{
			_db = db;
		}

		public CreatorIndexDTO? GetCreatorById(int creatorId)
		{
			return _db.Creators
				.Select(creator => new CreatorIndexDTO
				{
					Id = creator.Id,
					CreatorName = creator.CreatorName,
					CreatorPicPath = creator.CreatorPicPath,
				})
				.SingleOrDefault(dto => dto.Id == creatorId);
		}

        public Creator? GetCreatorByIdForCheck(int creatorId)
        {
			return _db.Creators.Find(creatorId);
        }

        public IEnumerable<CreatorIndexDTO> GetCreatorsByName(string name, int rowNumber)
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
				.Skip(rowNumber != 2 ? (rowNumber - 1) * skipNumber : 0)
				.Take(rowNumber != 2 ? takeNumber : takeNumber * 2)
				.ToList();
		}

		public IEnumerable<CreatorIndexDTO> GetLikedCreators(int memberId, MembersController.LikedQuery body)
		{
			var follows = _db.CreatorFollows.Where(follow => follow.MemberId == memberId);
			IEnumerable<Creator> creators = body.Condition switch
			{
				"RecentlyAdded" => follows
										.OrderByDescending(follow => follow.Created)
										.Select(follow => follow.Creator),
				"Alphatically" => follows
										.Select(follows => follows.Creator)
										.OrderBy(artist => artist.CreatorName),
				_ => (IEnumerable<Creator>)new List<Artist>(),
			};

			creators = body.RowNumber == 2 ?
				creators.Take(takeNumber * 2) :
				creators.Skip((body.RowNumber - 1) * skipNumber)
				.Take(takeNumber);

			return creators.Select(artist => new CreatorIndexDTO 
			{ 
				Id = artist.Id, 
				CreatorName = artist.CreatorName, 
				CreatorPicPath= artist.CreatorPicPath,
			});
		}
	}
}
