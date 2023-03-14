using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using static api.iSMusic.Controllers.MembersController;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class ArtistRepository : IRepository, IArtistRepository
	{
		private readonly AppDbContext _db;

		private readonly int skipNumber = 5;

		private readonly int takeNumber = 5;

		public ArtistRepository(AppDbContext db)
		{
			_db = db;
		}

        public IEnumerable<ArtistIndexDTO> GetRecommended()
        {
            var dtos = _db.Artists
                .Include(artist => artist.ArtistFollows)
                .Select(artist => new ArtistIndexDTO
				{
                    Id = artist.Id,
                    ArtistName = artist.ArtistName,
                    ArtistPicPath = artist.ArtistPicPath,
                    TotalFollows = artist.ArtistFollows.Count(),
                })
                .OrderByDescending(dto => dto.TotalFollows)
                .Take(takeNumber)
                .ToList();

            return dtos;
        }

        public Artist? GetArtistByIdForCheck(int artistId)
		{
			return _db.Artists.Find(artistId);
		}

		public ArtistIndexDTO? GetArtistById(int artistId)
		{
			return _db.Artists
				.Include(artist => artist.ArtistFollows)
				.Select(artist => new ArtistIndexDTO
				{
					Id = artist.Id,
					ArtistName= artist.ArtistName,
					ArtistPicPath= artist.ArtistPicPath,
					About = artist.ArtistAbout,
					TotalFollows = artist.ArtistFollows.Count(),
				})
				.SingleOrDefault(dto => dto.Id == artistId);
		}

		public IEnumerable<ArtistIndexDTO> GetArtistsByName(string artistName, int skipRows, int takeRows)
		{
			return _db.Artists
				.Where(artist => artist.ArtistName.Contains(artistName))
				.Select(artist => new ArtistIndexDTO
				{
					Id = artist.Id,
					ArtistName = artist.ArtistName,
					ArtistPicPath = artist.ArtistPicPath,
					TotalFollows = artist.ArtistFollows.Count(),
				})
				.OrderByDescending(dto => dto.TotalFollows)
				.Skip(skipRows)
				.Take(takeRows)
				.ToList();
		}

		public IEnumerable<ArtistIndexDTO> GetLikedArtists(int memberId, LikedQuery query)
		{
			var follows = _db.ArtistFollows.Where(follow => follow.MemberId == memberId);

			IEnumerable<Artist> artists = query.Condition switch
			{
				"RecentlyAdded" => follows
										.OrderByDescending(follow => follow.Created)
										.Select(follow => follow.Artist),
				"Alphabetically" => follows
										.Select(follows => follows.Artist)
										.OrderBy(artist => artist.ArtistName),
				_ => new List<Artist>(),
			};

			if (!string.IsNullOrEmpty(query.Input))
			{
				artists = artists.Where(artist => artist.ArtistName.Contains(query.Input));
			}

			artists = query.RowNumber == 2 ?
				artists.Take(takeNumber * 2) :
				artists.Skip((query.RowNumber - 1) * skipNumber)
				.Take(takeNumber);

			return artists.Select(artist => new ArtistIndexDTO 
			{ 
				Id = artist.Id, 
				ArtistName = artist.ArtistName, 
				ArtistPicPath= artist.ArtistPicPath,
			});
		}

		public ArtistAboutDTO GetArtistAbout(int artistId)
		{
			var about = _db.Artists
				.Include(artist => artist.ArtistFollows)
				.Select(artist => new ArtistAboutDTO
				{
					Id = artist.Id,
					ArtistName = artist.ArtistName,
					About = artist.ArtistAbout,
					Followers = artist.ArtistFollows.Count()
				})
				.Single(artist => artist.Id == artistId);

			var artistSongIds = _db.SongArtistMetadata
								.Where(metadata => metadata.ArtistId == artistId)
								.Select(metadata => metadata.SongId)
								.ToList();

			var monthlyPlayedTimes = _db.SongPlayedRecords
						.Where(record => artistSongIds.Contains(record.SongId) && record.PlayedDate.Month == DateTime.Now.Month)
						.Count();

			about.MonthlyPlayedTimes = monthlyPlayedTimes;

			return about;
		}
    }
}
