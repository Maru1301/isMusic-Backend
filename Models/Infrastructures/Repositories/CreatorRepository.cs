using api.iSMusic.Controllers;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public IEnumerable<CreatorIndexDTO> GetRecommended()
        {
            var dtos = _db.Creators
                .Include(creator => creator.CreatorFollows)
                .Select(creator => new CreatorIndexDTO
				{
                    Id = creator.Id,
                    CreatorName = creator.CreatorName,
                    CreatorPicPath = creator.CreatorPicPath,
                    TotalFollows = creator.CreatorFollows.Count,
                })
                .OrderByDescending(dto => dto.TotalFollows)
                .Take(takeNumber)
                .ToList();

            return dtos;
        }

        public CreatorIndexDTO? GetCreatorById(int creatorId)
		{
			return _db.Creators
				.Include(creator => creator.CreatorFollows)
				.Select(creator => new CreatorIndexDTO
				{
					Id = creator.Id,
					CreatorName = creator.CreatorName,
					CreatorPicPath = creator.CreatorPicPath,
					CreatorAbout = creator.CreatorAbout,
					TotalFollows = creator.CreatorFollows.Count,
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

		public IEnumerable<CreatorIndexDTO> GetLikedCreators(int memberId, MembersController.LikedQuery query)
		{
			var follows = _db.CreatorFollows.Where(follow => follow.MemberId == memberId);
			IEnumerable<Creator> creators = query.Condition switch
			{
				"RecentlyAdded" => follows
										.OrderByDescending(follow => follow.Created)
										.Select(follow => follow.Creator),
				"Alphabetically" => follows
										.Select(follows => follows.Creator)
										.OrderBy(artist => artist.CreatorName),
				_ => (IEnumerable<Creator>)new List<Artist>(),
			};

			if (!string.IsNullOrEmpty(query.Input))
			{
				creators = creators.Where(creator => creator.CreatorName.Contains(query.Input));
			}

			creators = query.RowNumber == 2 ?
				creators.Take(takeNumber * 2) :
				creators.Skip((query.RowNumber - 1) * skipNumber)
				.Take(takeNumber);

			return creators.Select(artist => new CreatorIndexDTO 
			{ 
				Id = artist.Id, 
				CreatorName = artist.CreatorName, 
				CreatorPicPath= artist.CreatorPicPath,
			});
		}

        public void CreateCreator(int memberId)
        {
            var member = _db.Members.Where(m => m.Id == memberId).FirstOrDefault();
            if (member.IsConfirmed)
            {
                var Creator = new Creator
                {
                    CreatorName = member.MemberNickName,
                    MemberId = memberId,
                    //CreatorPicPath = member.Avatar.Path

                };
                _db.Creators.Add(Creator);
                _db.SaveChanges();

            }
        }
    }
}
