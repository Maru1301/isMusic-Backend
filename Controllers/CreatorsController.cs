using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.DTOs.MusicDTOs;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Hosting.Internal;
using System;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CreatorsController : ControllerBase
	{
		private readonly ICreatorRepository _repository;

		private readonly CreatorService _service;
		//private readonly ISongRepository _songRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		

		public CreatorsController(ICreatorRepository repository, ISongRepository songRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository,IWebHostEnvironment webHostEnvironment)
		{
			_repository = repository;
			_service = new CreatorService(_repository, songRepository, albumRepository, playlistRepository);
			_webHostEnvironment = webHostEnvironment;
		}	


		[HttpGet]
		[Route("{creatorId}/Detail")]
		public ActionResult<ArtistDetailVM> GetCreatorDetail(int creatorId)
		{
			var result = _service.GetCreatorDetail(creatorId);
			if (!result.Success)
			{
				return NotFound(result.Message);
			}

			return Ok(result.dto.ToDetailVM());
		}

		[HttpGet]
		[Route("{creatorName}")]
		public ActionResult<IEnumerable<CreatorIndexVM>> GetCreatorsByName(string creatorName, [FromQuery]int rowNumber = 2)
		{
			var dtos = _service.GetCreatorsByName(creatorName, rowNumber);

			return Ok(dtos.Select(dto => dto.ToIndexVM()));
		}

        [HttpGet]
        [Route("{creatorId}/Albums")]
        public IActionResult GetCreatorAlbums(int creatorId, [FromQuery] int rowNumber = 2)
        {
            var result = _service.GetCreatorAlbums(creatorId, rowNumber);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Dtos.Select(dto => dto.ToIndexVM()));
        }

        [HttpGet]
        [Route("{creatorId}/Playlists")]
        public IActionResult GetCreatorPlaylists(int creatorId, [FromQuery] int rowNumber = 2)
        {
            var result = _service.GetCreatorPlaylists(creatorId, rowNumber);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Dtos.Select(dto => dto.ToIndexVM()));
        }

		[HttpPost]
		[Route("{creatorId}/song")]
		public IActionResult CreatorUploadSong(int creatorId,[FromForm] CreatorUploadSongDTO creatoruploadsongdto)
		{
			//string coverPath = "C:\\Users\\ispan\\Desktop\finalprojectapi\\Uploads\\Covers";
			//string songPath = "C:\\Users\\ispan\\Desktop\finalprojectapi\\Uploads\\Songs";
			//var result = _service.CreatorUploadSong(coverPath, songPath, creatoruploadsongdto);
			//return Ok(result.Message);
			var parentroot = Directory.GetParent(_webHostEnvironment.ContentRootPath).FullName;
			var uploadmusicpath = parentroot + @"/iSMusic.ServerSide/Uploads/Songs";
			var uploadcoverpath = parentroot + @"/iSMusic.ServerSide/Uploads/Covers";
			var musicfileName = 
			var coverfileName = coverfiles.FileName;
			using (var stream = System.IO.File.Create(uploadmusicpath + musicfileName))
				{
				musicfiles.CopyTo(stream);
				}
			using (var stream = System.IO.File.Create(uploadcoverpath + coverfileName))
			{
				coverfiles.CopyTo(stream);
			}
			
			
			
			return Ok("歌曲已上傳");
		}
    }
}
