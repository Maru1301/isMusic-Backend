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
		public AppDbContext _appDbContext;


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
			var musicfileName = GetNewFileName(uploadmusicpath, creatoruploadsongdto.Song.FileName);
			var coverfileName = GetNewFileName(uploadcoverpath, creatoruploadsongdto.Cover.FileName);
			using (var stream = System.IO.File.Create(uploadmusicpath + musicfileName))
				{
				creatoruploadsongdto.Song.CopyTo(stream);
				}
			using (var stream = System.IO.File.Create(uploadcoverpath + coverfileName))
			{
				creatoruploadsongdto.Cover.CopyTo(stream);
			}
			Song song = new()
			{
				Id = creatoruploadsongdto.Id,
				SongName=creatoruploadsongdto.Song.Name,
				GenreId=creatoruploadsongdto.GenreId,
				Duration= creatoruploadsongdto.Duration,
				IsInstrumental=creatoruploadsongdto.IsInstrumental,
				Language=creatoruploadsongdto.Language,
				IsExplicit=creatoruploadsongdto.IsExplicit,
				Released=creatoruploadsongdto.Released,
				SongWriter=creatoruploadsongdto.SongWriter,
				Lyric=creatoruploadsongdto.Lyric,
				SongCoverPath=creatoruploadsongdto.SongCoverPath,
				SongPath=creatoruploadsongdto.SongPath,
				Status=creatoruploadsongdto.Status,
				AlbumId=creatoruploadsongdto.AlbumId,

			};

			_appDbContext.Add(song);
			_appDbContext.SaveChanges();




			return Ok("歌曲已上傳");
		}
		private string GetNewFileName(string path, string fileName)
		{
			string ext = System.IO.Path.GetExtension(fileName); // 取得副檔名,例如".jpg"
			string newFileName;
			string fullPath;
			// todo use song name + artists name instead of guid, so when uploading the new file it will replace the old one.
			do
			{
				newFileName = Guid.NewGuid().ToString("N") + ext;
				fullPath = System.IO.Path.Combine(path, newFileName);
			} while (System.IO.File.Exists(fullPath) == true); // 如果同檔名的檔案已存在,就重新再取一個新檔名

			return newFileName;
		}
	}
}
