using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.SongVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ArtistsController : ControllerBase
	{
		private readonly IArtistRepository _repository;
		
		private readonly ArtistService _service;

		public ArtistsController(IArtistRepository repository, ISongRepository songrepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
			_repository = repository;
			_service = new ArtistService(_repository, songrepository, albumRepository, playlistRepository);
		}

		[HttpGet]
		[Route("Recommended")]
        public IActionResult GetRecommended()
        {
            var result = _service.GetRecommended();

            if (!result.Success)
            {
                return NoContent();
            }

            return Ok(result.Dtos.Select(dto => dto.ToIndexVM()));
        }

        [HttpGet]
		[Route("{artistId}/Detail")]
		public ActionResult<ArtistDetailVM> GetArtistDetail(int artistId)
		{
			int memberId = this.GetMemberId();
			var result = _service.GetArtistDetail(artistId, memberId);
			if (!result.Success)
			{
				return NotFound(result.Message);
			}

			return Ok(result.dto.ToDetailVM());
		}

		[HttpGet]
		[Route("{artistName}")]
		public ActionResult<IEnumerable<ArtistIndexVM>> GetArtistsByArtistName(string artistName, [FromQuery]int rowNumber = 2)
		{
			var dtos = _service.GetArtistsByName(artistName, rowNumber);

			return Ok(dtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{artistId}/Albums")]
		public IActionResult GetArtistAlbums(int artistId, [FromQuery]int rowNumber = 2)
		{
			var result = _service.GetArtistAlbums(artistId, rowNumber);
			if(!result.Success)
			{
				return NotFound(result.Message);
			}

			return Ok(result.Dtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{artistId}/Playlists")]
		public IActionResult GetArtistPlaylists(int artistId, [FromQuery]int rowNumber = 2)
		{
			var result = _service.GetArtistPlaylists(artistId, rowNumber);
			if (!result.Success)
			{
				return NotFound(result.Message);
			}

			return Ok(result.Dtos.Select(dto => dto.ToIndexVM()));
		}

		[HttpGet]
		[Route("{artistId}/About")]
		public IActionResult GetArtistAbout(int artistId)
		{
			var result = _service.GetArtistAbout(artistId);
			if (!result.Success)
			{
				return NotFound(result.Message);
			}

			return Ok(result.Dto.ToAboutVM());
		}
	}
}
