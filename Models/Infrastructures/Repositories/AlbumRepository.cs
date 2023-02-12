using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.AlbumVMs;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class AlbumRepository: IRepository, IAlbumRepository
	{
		private readonly AppDbContext _db;

		public AlbumRepository(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<AlbumIndexDTO> GetAlbumsByName(string albumName, int skipRows, int takeRows)
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
				.Skip(skipRows)
				.Take(takeRows)
				.ToList();
		}

		public IEnumerable<AlbumIndexDTO> GetAlbumsByGenreId(int genreId, int skipRows, int takeRows)
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
				.Skip(skipRows)
				.Take (takeRows)
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
				.Take(10)
				.ToList();

			return recommendedAlbums;
		}

		public Album? GetAlbumByIdForCheck(int albumId)
		{
			return _db.Albums.Find(albumId);
		}

		public AlbumIndexDTO? GetAlbumById(int albumId)
		{
			return _db.Albums
				.Select(album => new AlbumIndexDTO
				{
					Id = album.Id,
					AlbumName = album.AlbumName,
					AlbumCoverPath = album.AlbumCoverPath,
					AlbumTypeId = album.AlbumTypeId,
					AlbumGenreId = album.AlbumGenreId,
					Released = album.Released,
					MainArtistId = album.MainArtistId,
				})
				.SingleOrDefault(dto => dto.Id == albumId);
		}
	}
}
