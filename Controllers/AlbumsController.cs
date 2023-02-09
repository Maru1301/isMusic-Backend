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
		public IActionResult GetRecommended()
		{
			var recommendedAlbums = _db.Albums
				.Include(album => album.LikedAlbums)
				.Select(album => new AlbumIndexVM
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

			if (recommendedAlbums.Count() == 0)
			{
				return NoContent();
			}

			return Ok(recommendedAlbums);
		}
	}
}
