using api.iSMusic.Controllers;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.AlbumVMs;
using api.iSMusic.Models.ViewModels.SongVMs;
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

		public IEnumerable<AlbumIndexDTO> GetAlbumsByName(string albumName, int rowNumber = 1)
		{
			return _db.Albums
				.Where(album => album.AlbumName.Contains(albumName) && album.Released <= DateTime.Now)
                .Select(album => new AlbumIndexDTO
                {
                    Id = album.Id,
                    AlbumCoverPath = album.AlbumCoverPath,
                    AlbumName = album.AlbumName,
                    AlbumGenreId = album.AlbumGenreId,
                    AlbumGenreName = album.AlbumGenre.GenreName,
                    AlbumTypeId = album.AlbumTypeId,
                    AlbumTypeName = album.AlbumType.TypeName,
                    Released = album.Released,
                    MainArtistId = album.MainArtistId,
                    MainArtistName = album.MainArtist != null ?
                        album.MainArtist.ArtistName :
                        string.Empty,
                    MainCreatorId = album.MainCreatorId,
                    MainCreatorName = album.MainCreator != null ? album.MainCreator.CreatorName :
                        string.Empty,
                    TotalLikes = album.LikedAlbums.Count,
                })
                .OrderBy(dto => dto.TotalLikes)
				.Skip(rowNumber != 2 ? (rowNumber - 1) * skipNumber : 0)
				.Take(rowNumber != 2 ? takeNumber : takeNumber * 2)
				.ToList();
		}

		public IEnumerable<AlbumIndexDTO> GetAlbumsByGenreId(int genreId, int rowNumber = 1)
		{
			return _db.Albums
				.Where(album => album.AlbumGenreId== genreId)
                .Select(album => new AlbumIndexDTO
                {
                    Id = album.Id,
                    AlbumCoverPath = album.AlbumCoverPath,
                    AlbumName = album.AlbumName,
                    AlbumGenreId = album.AlbumGenreId,
                    AlbumGenreName = album.AlbumGenre.GenreName,
                    AlbumTypeId = album.AlbumTypeId,
                    AlbumTypeName = album.AlbumType.TypeName,
                    Released = album.Released,
                    MainArtistId = album.MainArtistId,
                    MainArtistName = album.MainArtist != null ?
                        album.MainArtist.ArtistName :
                        string.Empty,
                    MainCreatorId = album.MainCreatorId,
                    MainCreatorName = album.MainCreator != null ? album.MainCreator.CreatorName :
                        string.Empty,
                    TotalLikes = album.LikedAlbums.Count,
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
                    AlbumCoverPath = album.AlbumCoverPath,
                    AlbumName = album.AlbumName,
                    AlbumGenreId = album.AlbumGenreId,
                    AlbumGenreName = album.AlbumGenre.GenreName,
                    AlbumTypeId = album.AlbumTypeId,
                    AlbumTypeName = album.AlbumType.TypeName,
                    Released = album.Released,
                    MainArtistId = album.MainArtistId,
                    MainArtistName = album.MainArtist != null ?
                        album.MainArtist.ArtistName :
                        string.Empty,
                    MainCreatorId = album.MainCreatorId,
                    MainCreatorName = album.MainCreator != null ? album.MainCreator.CreatorName :
                        string.Empty,
                    TotalLikes = album.LikedAlbums.Count,
                })
				.OrderByDescending(x => x.TotalLikes)
				.Take(takeNumber)
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
				.Where(album => album.Id == albumId)
				.Include(album => album.AlbumGenre)
				.Include(album => album.AlbumType)
				.Include(album => album.MainArtist)
				.Include(album => album.MainCreator)
				.Include (album => album.Songs)
				.Include(album => album.Songs)
					.ThenInclude(song => song.SongArtistMetadata)
					.ThenInclude(metadata => metadata.Artist)
				.Include(album => album.Songs)
					.ThenInclude(song => song.SongCreatorMetadata)
					.ThenInclude(metadata => metadata.Creator)
				.Include(album => album.Songs)
					.ThenInclude(song => song.SongPlayedRecords)
				.Select(album => new AlbumDetailDTO
				{
					Id = album.Id,
					AlbumName = album.AlbumName,
					AlbumCoverPath = album.AlbumCoverPath,
					AlbumGenreId = album.AlbumGenreId,
					AlbumGenreName = album.AlbumGenre.GenreName,
					AlbumTypeId = album.AlbumTypeId,
					AlbumTypeName = album.AlbumType.TypeName,
					Released = album.Released,
					MainArtistId = album.MainArtistId,
					MainArtistName = album.MainArtist != null ? album.MainArtist.ArtistName : string.Empty,
					MainArtistPicPath = album.MainArtist!= null?
					album.MainArtist.ArtistPicPath : string.Empty,
					MainCreatorId = album.MainCreatorId,
					MainCreatorName = album.MainCreator != null ? album.MainCreator.CreatorName : string.Empty,
					MainCreatorPicPath = album.MainCreator != null?
					album.MainCreator.CreatorPicPath : string.Empty,
					Description = album.Description,
					AlbumProducer = album.AlbumProducer,
					AlbumCompany = album.AlbumCompany,
					Songs = album.Songs.Select(song => song.ToInfoDTO()).ToList(),
				})
				.SingleOrDefault();
        }

		public IEnumerable<AlbumIndexDTO> GetLikedAlbums(int memberId, MembersController.LikedQuery query)
		{
			var likeds = _db.LikedAlbums
				.Include(la => la.Album)
					.ThenInclude(album => album.AlbumGenre)
                .Include(la => la.Album)
                    .ThenInclude(album => album.AlbumType)
                .Include(la => la.Album)
                    .ThenInclude(album => album.MainArtist)
                .Include(la => la.Album)
                    .ThenInclude(album => album.MainCreator)
                .Where(liked => liked.MemberId == memberId);

			if(string.IsNullOrEmpty(query.Input) == false)
			{
				likeds = likeds.Where(liked => liked.Album.AlbumName.Contains(query.Input));
			}

			IEnumerable<Album> albums = query.Condition switch
			{
                "RecentlyAdded" => likeds.OrderByDescending(liked => liked.Created).Select(liked => liked.Album),
				"Alphabetically" => likeds.Select(liked => liked.Album)
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
				AlbumGenreName = album.AlbumGenre.GenreName,
				AlbumTypeId= album.AlbumTypeId,
				AlbumTypeName = album.AlbumType.TypeName,
				MainArtistId= album.MainArtistId,
				MainArtistName = album.MainArtist?.ArtistName,
				MainCreatorId= album.MainCreatorId,
				MainCreatorName = album.MainCreator?.CreatorName,
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
					AlbumCoverPath = album.AlbumCoverPath,
					AlbumName = album.AlbumName,
					AlbumGenreId = album.AlbumGenreId,
					AlbumGenreName = album.AlbumGenre.GenreName,
					AlbumTypeId = album.AlbumTypeId,
					AlbumTypeName = album.AlbumType.TypeName,
					Released = album.Released,
					MainArtistId = album.MainArtistId,
					MainArtistName = album.MainArtist != null ?
							album.MainArtist.ArtistName :
							string.Empty,
					MainCreatorId = album.MainCreatorId,
					MainCreatorName = album.MainCreator != null ?      album.MainCreator.CreatorName :
							string.Empty,
					TotalLikes = album.LikedAlbums.Count,
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
				.Select(album => album.ToIndexDTO())
				.OrderByDescending(dto => dto.TotalLikes)
				.Skip(rowNumber == 2 ? 0 : (rowNumber - 1) * skipNumber)
				.Take(takeNumber)
				.ToList();
		}

		public IEnumerable<AlbumTypeDTO> GetAlbumTypes()
		{
			return _db.AlbumTypes.Select(AlbumType => new AlbumTypeDTO
			{
				Id = AlbumType.Id,
				TypeName = AlbumType.TypeName
			});
		}

        public LikedAlbum? CheckIsLiked(int albumId, int memberId)
		{
			return _db.LikedAlbums.SingleOrDefault(la => la.AlbumId == albumId && la.MemberId == memberId);
		}
    }
}
