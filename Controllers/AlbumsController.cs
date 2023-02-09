using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.AlbumVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AlbumsController : ControllerBase
	{
		private readonly AppDbContext _db;

		public AlbumsController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		[Route("Recommended")]
		public ActionResult<AlbumIndexVM> GetRecommended()
		{
			var data = _db.Albums
				.Include(album => album.LikedAlbums)
				.Select(album => new
				{
					album.Id,
					album.AlbumName,
					album.AlbumCoverPath,
					album.AlbumTypeId,
					album.AlbumGenreId,
					album.Released,
					album.MainArtistId,
					TotalLiked = album.LikedAlbums.Count(),
				}).OrderByDescending(x => x.TotalLiked).Take(10)
				.Select(x => new AlbumIndexVM
				{
					Id = x.Id,
					AlbumName = x.AlbumName,
					AlbumCoverPath = x.AlbumCoverPath,
					AlbumTypeId = x.AlbumTypeId,
					AlbumGenreId = x.AlbumGenreId,
					Released = x.Released,
					MainArtistId = x.MainArtistId,
				});

			return Ok(data);
		}
	}
}
