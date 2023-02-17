using api.iSMusic.Controllers;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.AlbumVMs;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class AlbumRepository: IRepository, IAlbumRepository
	{
		private readonly AppDbContext _db;

		private readonly int skipNumber = 5;

		private readonly int takeNumber = 5;

		public AlbumRepository(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<AlbumIndexDTO> GetAlbumsByName(string albumName, int rowNumber)
		{
			return _db.Albums
				.Where(album => album.AlbumName.Contains(albumName) && album.Released <= DateTime.Now)
				.Select(album => new AlbumIndexDTO 
				{
					Id= album.Id,
					AlbumCoverPath = album.AlbumCoverPath,
					AlbumName = album.AlbumName,
					AlbumGenreId = album.AlbumGenreId,
					AlbumTypeId = album.AlbumTypeId,
					Released = album.Released,
					MainArtistId = album.MainArtistId,
					TotalLikes = album.LikedAlbums.Count,
				})
				.OrderBy(dto => dto.TotalLikes)
				.Skip(rowNumber != 2 ? (rowNumber - 1) * skipNumber : 0)
				.Take(rowNumber != 2 ? takeNumber : takeNumber * 2)
				.ToList();
		}

		public IEnumerable<AlbumIndexDTO> GetAlbumsByGenreId(int genreId, int rowNumber)
		{
			return _db.Albums
				.Where(album => album.AlbumGenreId== genreId)
				.Select(album => new AlbumIndexDTO
				{
					Id= album.Id,
					AlbumName= album.AlbumName,
					AlbumGenreId= album.AlbumGenreId,
					AlbumCoverPath= album.AlbumCoverPath,
					AlbumTypeId= album.AlbumTypeId,
					Released= album.Released,
					MainArtistId= album.MainArtistId,
					TotalLikes= album.LikedAlbums.Count,
				})
				.OrderBy(dto => dto.TotalLikes)
				.Skip(rowNumber != 2 ? (rowNumber - 1) * skipNumber : 0)
				.Take(rowNumber != 2 ? takeNumber : takeNumber * 2)
				.ToList();
		}

		public IEnumerable<AlbumIndexDTO> GetRecommended()
		{
			var recommendedAlbums = _db.Albums
				.Include(album => album.LikedAlbums)
				.Select(album => new AlbumIndexDTO
				{
					Id = album.Id,
					AlbumName = album.AlbumName,
					AlbumCoverPath = album.AlbumCoverPath,
					AlbumTypeId = album.AlbumTypeId,
					AlbumGenreId = album.AlbumGenreId,
					Released = album.Released,
					MainArtistId = album.MainArtistId,
					TotalLikes = album.LikedAlbums.Count()
				})
				.OrderByDescending(x => x.TotalLikes)
				.Take(takeNumber * 2)
				.ToList();

			return recommendedAlbums;
		}

		public Album? GetAlbumByIdForCheck(int albumId)
		{
			return _db.Albums.Find(albumId);
		}

		public AlbumDetailDTO? GetAlbumById(int albumId)
		{
			return _db.Albums
				.Select(album => new AlbumDetailDTO
				{
					Id = album.Id,
					AlbumName = album.AlbumName,
					AlbumCoverPath = album.AlbumCoverPath,
					AlbumTypeId = album.AlbumTypeId,
					AlbumTypeName = album.AlbumType.TypeName,
					AlbumGenreId = album.AlbumGenreId,
					AlbumGenreName = album.AlbumGenre.GenreName,
					Released = album.Released,
					MainArtistId = album.MainArtistId,
					MainArtistName = album.MainArtist != null ?
						album.MainArtist.ArtistName:
						string.Empty,
					MainCreatorId= album.MainCreatorId,
					MainCreatorName = album.MainCreator != null ?					album.MainCreator.CreatorName: 
						string.Empty,
					Description = album.Description,
					AlbumProducer = album.AlbumProducer,
					AlbumCompany= album.AlbumCompany,
					Songs = album.Songs.Select(song => song.ToInfoDTO()).ToList(),
				})
				.SingleOrDefault(dto => dto.Id == albumId);
		}

		public IEnumerable<AlbumIndexDTO> GetLikedAlbums(int memberId, MembersController.LikedQuery query)
		{
			var likeds = _db.LikedAlbums.Where(liked => liked.MemberId == memberId);

			IEnumerable<Album> albums = query.Condition switch
			{
				"RecentlyAdded" => likeds
										.OrderByDescending(liked => liked.Created)
										.Select(liked => liked.Album),
				"Alphatically" => likeds
										.Select(liked => liked.Album)
										.OrderBy(album => album.AlbumName),
				_ => new List<Album>(),
			};

			albums = query.RowNumber == 2 ?
				albums.Take(takeNumber * 2) :
				albums.Skip((query.RowNumber - 1) * skipNumber)
				.Take(takeNumber);

			return albums.Select(album => new AlbumIndexDTO
			{
				Id = album.Id,
				AlbumName = album.AlbumName,
				AlbumCoverPath = album.AlbumCoverPath,
				AlbumGenreId= album.AlbumGenreId,
				AlbumTypeId= album.AlbumTypeId,
				MainArtistId= album.MainArtistId,
				Released = album.Released,
			});
		}

		public IEnumerable<AlbumIndexDTO> GetPopularAlbums(int contentId, string mode, int rowNumber = 1)
		{
			var albums = _db.Albums
				.Include(album => album.LikedAlbums)
				.Where(album => album.Released <= DateTime.Now);

			if(mode == "Artist")
			{
				albums = albums.Where(album => album.MainArtistId == contentId);
			}
			else if(mode == "Creator")
			{
				albums = albums.Where(album => album.MainCreatorId == contentId);
			}

			var dtos = albums.Select(album => new AlbumIndexDTO
				{
					Id = album.Id,
					AlbumName = album.AlbumName,
					AlbumGenreId= album.AlbumGenreId,
					AlbumTypeId= album.AlbumTypeId,
					AlbumCoverPath= album.AlbumCoverPath,
					Released = album.Released,
					MainArtistId = album.MainArtistId,
					MainArtistName = album.MainArtist != null ?						album.MainArtist.ArtistName:
						null,
					MainCreatorId = album.MainCreatorId,
					MainCreatorName = album.MainCreator != null ?
						album.MainCreator.CreatorName :
						null,
					TotalLikes = album.LikedAlbums.Count(),
				})
				.OrderByDescending(dto => dto.TotalLikes)
				.Skip(rowNumber == 2 ? 0 : (rowNumber - 1) * skipNumber)
				.Take(rowNumber == 2 ? takeNumber * 2 : takeNumber)
				.ToList();

			return dtos;
		}

		public IEnumerable<AlbumIndexDTO> GetAlbumsByContentId(int contentId, string mode, int rowNumber)
		{
			return _db.Albums
				.Where(album => (mode == "Artist") ? album.MainArtistId == contentId : album.MainCreatorId == contentId)
				.Include(album => album.LikedAlbums)
				.Select(album => new AlbumIndexDTO
				{
					Id = album.Id,
					AlbumName = album.AlbumName,
					AlbumGenreId = album.AlbumGenreId,
					AlbumTypeId = album.AlbumTypeId,
					AlbumCoverPath = album.AlbumCoverPath,
					Released = album.Released,
					MainArtistId = album.MainArtistId,
					MainArtistName = album.MainArtist != null ? album.MainArtist.ArtistName :
						null,
					MainCreatorId = album.MainCreatorId,
					MainCreatorName = album.MainCreator != null ?
						album.MainCreator.CreatorName :
						null,
					TotalLikes = album.LikedAlbums.Count(),
				})
				.OrderByDescending(dto => dto.TotalLikes)
				.Skip(rowNumber == 2 ? 0 : (rowNumber - 1) * skipNumber)
				.Take(takeNumber)
				.ToList();
		}
	}
}
