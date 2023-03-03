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
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
    [Route("[controller]")]
	[ApiController]
	public class CreatorsController : ControllerBase
	{
		private readonly ICreatorRepository _repository;
		private readonly CreatorService _service;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly AppDbContext _appDbContext;


		public CreatorsController(ICreatorRepository repository, ISongRepository songRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository,IWebHostEnvironment webHostEnvironment,AppDbContext appDbContext)
		{
			_repository = repository;
			_service = new CreatorService(_repository, songRepository, albumRepository, playlistRepository);
			_webHostEnvironment = webHostEnvironment;
			_appDbContext = appDbContext;
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
        [Route("{creatorId}/PopularAlbums")]
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
		[Route("{creatorId}/song")]//創作者上傳歌曲
		public IActionResult CreatorUploadSong(int creatorId,[FromForm] CreatorUploadSongDTO creatoruploadsongdto)
		{
			var parentroot = Directory.GetParent(_webHostEnvironment.ContentRootPath)!.FullName;
			var uploadmusicpath = parentroot + @"/iSMusic.ServerSide/iSMusic/Uploads/Songs/";
			var uploadcoverpath = parentroot + @"/iSMusic.ServerSide/iSMusic/Uploads/Covers/";
			var musicfileName = GetNewFileName(uploadmusicpath, creatoruploadsongdto.Song!.FileName);
			var coverfileName = GetNewFileName(uploadcoverpath, creatoruploadsongdto.Cover!.FileName);
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
				
				SongName=creatoruploadsongdto.Song.Name,
				GenreId=creatoruploadsongdto.GenreId,
				Duration= creatoruploadsongdto.Duration,
				IsInstrumental=creatoruploadsongdto.IsInstrumental,
				Language=creatoruploadsongdto.Language,
				IsExplicit=creatoruploadsongdto.IsExplicit,
				Released=creatoruploadsongdto.Released,
				SongWriter=creatoruploadsongdto.SongWriter,
				Lyric=creatoruploadsongdto.Lyric,
				SongCoverPath= coverfileName,
				SongPath = musicfileName,
				Status =creatoruploadsongdto.Status,
				AlbumId=creatoruploadsongdto.AlbumId,

			};
			_appDbContext.Add(song);
			_appDbContext.SaveChanges();
			var songid = _appDbContext.Songs.OrderByDescending(s=>s.Id).First().Id;

			SongCreatorMetadatum songCreatormetadatum = new()
			{
				CreatorId = creatorId,
				SongId = songid
			};
						
			_appDbContext.Add(songCreatormetadatum);
			_appDbContext.SaveChanges();

			return Ok("歌曲已上傳");
		}
		private string GetNewFileName(string path, string fileName)//自動生成新檔案名
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

		
		[HttpGet]
		[Route("{creatorId}/songs")]//取得創作者的所有歌曲
		public IActionResult GetSongsByCreator(int creatorId)
		{
			var songs = _appDbContext.Songs
				.Where(s => s.SongCreatorMetadata.Any(sm => sm.CreatorId == creatorId))
				.Select(s => new SongDTO
				{
					Id = s.Id,
					SongName = s.SongName,
					GenreId = s.GenreId,
					Duration = s.Duration,
					IsInstrumental = s.IsInstrumental,
					Language = s.Language,
					IsExplicit = s.IsExplicit,
					Released = s.Released,
					SongWriter = s.SongWriter,
					Lyric = s.Lyric,
					SongCoverPath = s.SongCoverPath,
					SongPath = s.SongPath,
					Status = s.Status,
					AlbumId = s.AlbumId
				}).ToList();

			if (songs == null || songs.Count == 0)
			{
				return NotFound("您目前尚未擁有作品");
			}

			return Ok(songs);
		}


		[HttpPost]
		[Route("{creatorId}/albums")]//創作者新增專輯
		public IActionResult CreateAlbumbyCreator(int creatorId, [FromForm] CreateAlbumbyCreatorDTO dto)
		{
			// 檢查 Creator 是否存在
			var creator = _appDbContext.Creators.SingleOrDefault(c => c.Id == creatorId);
			if (creator == null)
			{
				return NotFound();
			}
			//上傳專輯封面
			var parentroot = Directory.GetParent(_webHostEnvironment.ContentRootPath)!.FullName;
			var uploadcoverpath = parentroot + @"/iSMusic.ServerSide/iSMusic/Uploads/Songs/";
			var coverfileName = GetNewFileName(uploadcoverpath, dto.Cover!.FileName);
			using (var stream = System.IO.File.Create(uploadcoverpath + coverfileName))
			{
				dto.Cover.CopyTo(stream);
			}
			
			// 將 AlbumDTO 轉換成 Album Entity
			Album album = new()
			{
				AlbumName = dto.AlbumName,
				AlbumCoverPath = coverfileName,
				AlbumTypeId = dto.AlbumTypeId,
				AlbumGenreId = dto.AlbumGenreId,
				Released = dto.Released,
				Description =	dto.Description,
				MainArtistId = dto.MainArtistId,
				MainCreatorId = creatorId,
				AlbumProducer = dto.AlbumProducer,
				AlbumCompany = dto.AlbumCompany
			};
			
			// 將 Album Entity 加入資料庫
			_appDbContext.Albums.Add(album);
			_appDbContext.SaveChanges();
						
			return Ok("專輯已建立");
		}

		[HttpGet]
		[Route("{creatorId}/albums")]//取得創作者的所有專輯
		public IActionResult GetAlbumsByCreator(int creatorId)
		{
			var albums = _appDbContext.Albums
				.Where(a => a.MainCreatorId == creatorId)
				.ToList();

			if (albums.Count == 0)
			{
				return NotFound();
			}

			var albumDTOs = new List<AlbumDTO>();

			foreach (var album in albums)
			{
				var albumDTO = new AlbumDTO
				{
					Id = album.Id,
					AlbumName = album.AlbumName,
					AlbumCoverPath = album.AlbumCoverPath,
					AlbumTypeId = album.AlbumTypeId,
					AlbumGenreId = album.AlbumGenreId,
					Released = album.Released,
					Description = album.Description,
					MainArtistId= album.MainArtistId,
					MainCreatorId= album.MainCreatorId,
					AlbumProducer= album.AlbumProducer,
					AlbumCompany= album.AlbumCompany,
				};

				albumDTOs.Add(albumDTO);
			}

			return Ok(albumDTOs);
		}

		[HttpDelete]
		[Route("{creatorId}/album/{albumId}")]//刪除創作者的單一專輯
		public IActionResult DeleteAlbumByCreator(int creatorId, int albumId)
		{
			var album = _appDbContext.Albums
				.Where(a => a.MainCreatorId == creatorId && a.Id == albumId)
				.FirstOrDefault();

			if (album == null)
			{
				return NotFound();
			}

			_appDbContext.Albums.Remove(album);
			_appDbContext.SaveChanges();

			return Ok();
		}

		[HttpDelete]
		[Route("{creatorId}/song_creator_Metadata/{songId}")]//刪除創作者的單一歌曲
		public IActionResult DeleteSongByCreator(int creatorId, int songId)
		{
			//確認創作者存不存在
			var creator = _appDbContext.Creators.FirstOrDefault(c => c.Id == creatorId);
			if (creator == null)
			{
				return NotFound();
			}
			//確認歌曲存不存在
			var song = _appDbContext.Songs.FirstOrDefault(s => s.Id == songId);
			if (song == null)
			{
				return NotFound();
			}
			//確認歌曲是否屬於該創作者
			var creatorSong = _appDbContext.SongCreatorMetadata.FirstOrDefault(sc => sc.CreatorId == creatorId && sc.SongId == songId);
			if (creatorSong == null)
			{
				return NotFound();
			}
			//刪除metadata資料
			_appDbContext.SongCreatorMetadata.Remove(creatorSong);
			_appDbContext.SaveChanges();
			//刪除歌曲
			_appDbContext.Songs.Remove(song);
			_appDbContext.SaveChanges();
			

			return NoContent();
		}

		[HttpPut]
		[Route("{creatorId}/songs/{songId}")]//修改創作者的單一歌曲
		public IActionResult EditSongByCreator(int creatorId,int songId, [FromBody]SongDTO dto)
		{
			
			//確認歌曲是否屬於該創作者
			var creatorSong = _appDbContext.SongCreatorMetadata.FirstOrDefault(sc => sc.CreatorId == creatorId && sc.SongId == songId);
			if (creatorSong == null)
			{
				return NotFound();
			}
			//抓出歌曲並修改值
			var song = _appDbContext.Songs.FirstOrDefault(s => s.Id == songId);

			song!.SongName = dto.SongName!;
			song.GenreId = dto.GenreId;
			//song.Duration = dto.Duration;
			song.IsInstrumental = dto.IsInstrumental;
			song.Language = dto.Language;
			song.IsExplicit = dto.IsExplicit;
			song.Released = dto.Released;
			song.SongWriter = dto.SongWriter!;
			song.Lyric = dto.Lyric;
			//song.SongCoverPath = dto.SongCoverPath;
			//song.SongPath = dto.SongPath;
			song.Status = dto.Status;
			song.AlbumId = dto.AlbumId ==0?null:dto.AlbumId;

			//存檔
			_appDbContext.SaveChanges();
			return NoContent();

		}

		
		[HttpGet]
		[Route("{creatorId}/albums/{albumId}")]//取得創作者的單一專輯
		public IActionResult GetAlbumByCreator(int creatorId, int albumId)
		{
			// 確認創作者是否存在
			var creator = _appDbContext.Creators.FirstOrDefault(c => c.Id == creatorId);
			if (creator == null)
			{
				return NotFound();
			}

			// 確認專輯是否存在
			var album = _appDbContext.Albums.FirstOrDefault(a => a.Id == albumId && a.MainCreatorId == creatorId);
			if (album == null)
			{
				return NotFound();
			}

			// 取得專輯的所有歌曲
			var songs = _appDbContext.Songs.Where(s => s.AlbumId == albumId);

			// 將歌曲轉換成ViewModel
			var songViewModels = songs.Select(s => new SongViewModel
			{
				Id = s.Id,
				Name = s.SongName,
				GenreId = s.GenreId,
				Duration = s.Duration,
				IsInstrumental = s.IsInstrumental,
				Language = s.Language,
				IsExplicit = s.IsExplicit,
				Released = s.Released,
				SongWriter = s.SongWriter,
				Lyric = s.Lyric,
				SongCoverPath = s.SongCoverPath,
				SongPath = s.SongPath,
				Status = s.Status,
				AlbumId = s.AlbumId
			}).ToList();

			// 將創作者和專輯的資訊轉換成ViewModel
			var albumViewModel = new AlbumViewModel
			{
				Id = album.Id,
				Name = album.AlbumName,
				CoverPath = album.AlbumCoverPath,
				AlbumTypeId = album.AlbumTypeId,
				AlbumGenreId = album.AlbumGenreId,
				Released = album.Released,
				Description = album.Description,
				MainArtistId = album.MainArtistId,
				MainCreatorId = album.MainCreatorId,
				AlbumProducer = album.AlbumProducer,
				AlbumCompany = album.AlbumCompany,
				Creator = new CreatorViewModel
				{
					Id = creator.Id,
					Name = creator.CreatorName,
					Gender = creator.CreatorGender,
					About = creator.CreatorAbout,
					PicPath = creator.CreatorPicPath,
					CoverPath = creator.CreatorCoverPath
				},
				Songs = songViewModels
			};

			return Ok(albumViewModel);
		}

		//修改創作者的單一專輯 TODO
	}
}
