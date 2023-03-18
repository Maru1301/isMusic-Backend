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
using api.iSMusic.Models.ViewModels.AlbumVMs;
using System.Runtime.InteropServices;
using api.iSMusic.Models.ViewModels.SongVMs;
using Microsoft.AspNetCore.StaticFiles;
using NAudio.Wave;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

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


        public CreatorsController(ICreatorRepository repository, ISongRepository songRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository, IWebHostEnvironment webHostEnvironment, AppDbContext appDbContext)
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
        public IActionResult GetCreatorDetail(int creatorId)
        {
            int memberId = this.GetMemberId();
            var result = _service.GetCreatorDetail(creatorId, memberId);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result.dto.ToDetailVM());
        }

        [HttpGet]
        [Route("{creatorName}")]
        public ActionResult<IEnumerable<CreatorIndexVM>> GetCreatorsByName(string creatorName, [FromQuery] int rowNumber = 2)
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

        [HttpGet]
        [Route("CreatorPage")]//取得創作者個人資訊
        public IActionResult GetCreatorById()
        {
            int memberId = this.GetMemberId();
            var creatortotalfollows = _appDbContext.CreatorFollows.GroupBy(follow => follow.CreatorId)
                   .Select(group => new { CreatorId = group.Key, Count = group.Count() });


            var creator = _appDbContext.Creators
                .Include(c => c.Member)
                .Include(c => c.CreatorFollows)
                .Where(c => c.MemberId == memberId)
                .Select(c => new CreatorDTO
                {
                    Id = c.Id,
                    CreatorName = c.CreatorName,
                    CreatorAbout = c.CreatorAbout,
                    CreatorCoverPath = c.CreatorCoverPath,
                    CreatorPicPath = c.CreatorPicPath,
                    TotalFollows = c.CreatorFollows.Count()
                    //creatortotalfollows.Where(f => f.CreatorId == c.Id).Select(f => f.Count).FirstOrDefault(),
                }).FirstOrDefault();
            if(creator == null)
            {
                return NotFound("Creator not found");
            }

            //var parentroot = Directory.GetParent(_webHostEnvironment.ContentRootPath)!.FullName;
            var uploadpicpath =  "https://localhost:44373/Uploads/Covers/";
            var uploadcoverpath = "https://localhost:44373/Uploads/Covers/";
            if(!string.IsNullOrEmpty(creator.CreatorPicPath))
            {
                    creator.CreatorPic = uploadpicpath + creator.CreatorPicPath;
            }

            if (!string.IsNullOrEmpty(creator.CreatorCoverPath))
            {
                    creator.CreatorCover = uploadcoverpath + creator.CreatorCoverPath;
            }
           

            return Ok(creator);
        }

        [HttpGet]
        [Route("{creatorId}/songs")]//取得創作者的所有歌曲
        public IActionResult GetSongsByCreator(int creatorId)
        {
            try
            {


                var songs = _appDbContext.Songs.AsNoTracking()
                    .Include(a => a.Genre)
                    .Include(a => a.Album)
                    .Where(s => s.SongCreatorMetadata.Any(sm => sm.CreatorId == creatorId))
                    .Select(s => new SongDTO
                    {
                        Id = s.Id,
                        SongName = s.SongName,
                        GenreId = s.GenreId,
                        GenreName = s.Genre.GenreName,
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
                        //AlbumId = s.AlbumId,
                        AlbumName = s.Album!.AlbumName
                    }).ToList();

                var parentroot = Directory.GetParent(_webHostEnvironment.ContentRootPath)!.FullName;
                var uploadmusicpath = parentroot + @"\iSMusic.ServerSide\iSMusic\Uploads\Songs\";
                var uploadcoverpath = parentroot + @"\iSMusic.ServerSide\iSMusic\Uploads\Covers\";
                foreach (var s in songs)
                {
                    //if (System.IO.File.Exists(uploadmusicpath + s.SongPath))
                    //{
                    //    s.Song = System.IO.File.ReadAllBytes(uploadmusicpath + s.SongPath);
                    //}
                    if (System.IO.File.Exists(uploadcoverpath + s.SongCoverPath))
                    {
                        s.Cover = Convert.ToBase64String(System.IO.File.ReadAllBytes(uploadcoverpath + s.SongCoverPath));
                    }
                }

                //if (songs == null || songs.Count == 0)
                //{
                //    return NotFound("您目前尚未擁有作品");
                //}

                return Ok(songs.Any() ? JsonConvert.SerializeObject(songs) : JsonConvert.SerializeObject(new List<SongDTO>()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            var album = _appDbContext.Albums.Include(a => a.AlbumGenre).Include(a => a.AlbumType)
                .FirstOrDefault(a => a.Id == albumId && a.MainCreatorId == creatorId);
            if (album == null)
            {
                return NotFound();
            }

            // 取得專輯的所有歌曲
            var songs = _appDbContext.Songs
                .Include(s => s.Genre).Include(s => s.Album)
                .Where(s => s.AlbumId == albumId);

            // 將歌曲轉換成ViewModel
            var songVM = songs.Select(s => new CreatorSongVM
            {
                Name = s.SongName,
                GenredName = s.Genre.GenreName,
                Duration = s.Duration,
                IsInstrumental = s.IsInstrumental,
                Language = s.Language!,
                IsExplicit = s.IsExplicit,
                Released = s.Released,
                SongWriter = s.SongWriter,
                Lyric = s.Lyric!,
                Status = s.Status,
                AlbumName = s.Album!.AlbumName,
            }).ToList();

            //將創作者和專輯的資訊轉換成ViewModel
            CreatorAlbumVM creatoralbumVM = new()
            {
                AlbumName = album.AlbumName,
                AlbumCoverPath = album.AlbumCoverPath,
                AlbumTypeName = album.AlbumType.TypeName,
                AlbumGenreName = album.AlbumGenre.GenreName,
                Released = album.Released,
                Description = album.Description,
                AlbumProducer = album.AlbumProducer,
                AlbumCompany = album.AlbumCompany,
                Songs = songVM,
            };

            return Ok(creatoralbumVM);
        }

        [HttpGet]
        [Route("{creatorId}/albums")]//取得創作者的所有專輯
        public IActionResult GetAlbumsByCreator(int creatorId)
        {
            var albums = _appDbContext.Albums
                .Include(a => a.AlbumType)
                .Include(a => a.AlbumGenre)
                .Include(a => a.MainCreator)
                .Where(a => a.MainCreatorId == creatorId).Select(album => new AlbumDTO
                {
                    Id = album.Id,
                    AlbumName = album.AlbumName,
                    AlbumCoverPath = album.AlbumCoverPath,
                    AlbumTypeId = album.AlbumTypeId,
                    AlbumTypeName = album.AlbumType.TypeName,
                    AlbumGenreId = album.AlbumGenreId,
                    AlbumGenreName = album.AlbumGenre.GenreName,
                    Released = album.Released,
                    Description = album.Description,
                    MainArtistId = album.MainArtistId,
                    MainCreatorId = album.MainCreatorId,
                    MainCreatorName = album.MainCreator!.CreatorName,
                    AlbumProducer = album.AlbumProducer,
                    AlbumCompany = album.AlbumCompany,
                    SongList = album.Songs.Select(x => x.Id).ToArray(),
                }).ToList();
            var parentroot = Directory.GetParent(_webHostEnvironment.ContentRootPath)!.FullName;
            //  var uploadmusicpath = parentroot + @"\iSMusic.ServerSide\iSMusic\Uploads\Songs\";
            var uploadcoverpath = parentroot + @"\iSMusic.ServerSide\iSMusic\Uploads\Covers\";
            foreach (var s in albums)
            {
                //if (System.IO.File.Exists(uploadmusicpath + s.SongPath))
                //{
                //    s.Song = System.IO.File.ReadAllBytes(uploadmusicpath + s.SongPath);
                //}
                if (System.IO.File.Exists(uploadcoverpath + s.AlbumCoverPath))
                {
                    s.Cover = Convert.ToBase64String(System.IO.File.ReadAllBytes(uploadcoverpath + s.AlbumCoverPath));
                }
            }
            //if (albums.Count == 0)
            //{
            //    return NotFound();
            //}

            //var albumDTOs = new List<AlbumDTO>();

            //foreach (var album in albums)
            //{
            //    var albumDTO = new AlbumDTO
            //    {
            //        Id = album.Id,
            //        AlbumName = album.AlbumName,
            //        AlbumCoverPath = album.AlbumCoverPath,
            //        AlbumTypeId = album.AlbumTypeId,
            //        AlbumTypeName = album.AlbumType.TypeName,
            //        AlbumGenreId = album.AlbumGenreId,
            //        AlbumGenreName = album.AlbumGenre.GenreName,
            //        Released = album.Released,
            //        Description = album.Description,
            //        MainArtistId = album.MainArtistId,
            //        MainCreatorId = album.MainCreatorId,
            //        MainCreatorName = album.MainCreator!.CreatorName,
            //        AlbumProducer = album.AlbumProducer,
            //        AlbumCompany = album.AlbumCompany,
            //    };

            //    albumDTOs.Add(albumDTO);
            //}

            return Ok(albums.Any() ? JsonConvert.SerializeObject(albums) : JsonConvert.SerializeObject(new List<AlbumDTO>()));
        }

        [HttpPost]
        [Route("{creatorId}/song")]//創作者上傳歌曲
        public IActionResult CreatorUploadSong(int creatorId, [FromForm] CreatorUploadSongDTO creatoruploadsongdto)
        {
            try
            {
                var parentroot = Directory.GetParent(_webHostEnvironment.ContentRootPath)!.FullName;
                var uploadmusicpath = parentroot + @"\iSMusic.ServerSide\iSMusic\Uploads\Songs\";
                var uploadcoverpath = parentroot + @"\iSMusic.ServerSide\iSMusic\Uploads\Covers\";
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
                //取得音樂長度
                var audioFile = new AudioFileReader(uploadmusicpath + musicfileName);
                var duration = Convert.ToInt32(audioFile.TotalTime.TotalSeconds);
                Song song = new()
                {

                    SongName = creatoruploadsongdto.SongName,
                    GenreId = creatoruploadsongdto.GenreId,
                    Duration = (int)duration,
                    IsInstrumental = creatoruploadsongdto.IsInstrumental,
                    Language = creatoruploadsongdto.Language,
                    IsExplicit = creatoruploadsongdto.IsExplicit,
                    Released = creatoruploadsongdto.Released,
                    SongWriter = creatoruploadsongdto.SongWriter,
                    Lyric = creatoruploadsongdto.Lyric,
                    SongCoverPath = coverfileName,
                    SongPath = musicfileName,
                    Status = creatoruploadsongdto.Status,
                    AlbumId = creatoruploadsongdto.AlbumId,

                };
                _appDbContext.Add(song);
                _appDbContext.SaveChanges();
                var songid = _appDbContext.Songs.OrderByDescending(s => s.Id).First().Id;

                SongCreatorMetadatum songCreatormetadatum = new()
                {
                    CreatorId = creatorId,
                    SongId = songid
                };

                _appDbContext.Add(songCreatormetadatum);
                _appDbContext.SaveChanges();

                return Ok("歌曲已上傳");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);

            }
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
            var uploadcoverpath = parentroot + @"/iSMusic.ServerSide/iSMusic/Uploads/Covers/";
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
                Description = dto.Description,
                MainArtistId = dto.MainArtistId,
                MainCreatorId = creatorId,
                AlbumProducer = dto.AlbumProducer,
                AlbumCompany = dto.AlbumCompany,
            };
            var songs = _appDbContext.Songs.Where(x => dto.SongList.Contains(x.Id));
            if (songs.Any())
            {
                foreach (var song in songs)
                {
                    album.Songs.Add(song);
                }
            }
            // 將 Album Entity 加入資料庫
            _appDbContext.Albums.Add(album);
            _appDbContext.SaveChanges();

            return Ok("專輯已建立");
        }

        [HttpPut]
        [Route("CreatorPage")]//創作者編輯個人資料	
        public IActionResult CreatorUpdateProfile( [FromForm] CreatorUpdateProfileDTO dto)
        {
            string? picfileName, coverfileName;
            picfileName = coverfileName = null;


			int memberId = this.GetMemberId();
            var creator = _appDbContext.Creators.SingleOrDefault(x => x.MemberId == memberId);
            picfileName = creator.CreatorPicPath;
            coverfileName = creator.CreatorCoverPath;
            if (creator == null)
            {
                return NotFound();
            }
            int creatorId = creator.Id;
			var parentroot = Directory.GetParent(_webHostEnvironment.ContentRootPath)!.FullName;
			if (dto.Pic !=null)
            {
				var uploadpicpath = parentroot + @"/iSMusic.ServerSide/iSMusic/Uploads/Covers/";
				 picfileName = GetNewFileName(uploadpicpath, dto.Pic.FileName);
				using (var stream = System.IO.File.Create(uploadpicpath + picfileName))
				{
					dto.Pic.CopyTo(stream);
				}

            }
            
            if(dto.Cover != null)
            {
				var uploadcoverpath = parentroot + @"/iSMusic.ServerSide/iSMusic/Uploads/Covers/";
				 coverfileName = GetNewFileName(uploadcoverpath, dto.Cover.FileName);
				using (var stream = System.IO.File.Create(uploadcoverpath + coverfileName))
				{
					dto.Cover.CopyTo(stream);
				}
			}



            creator.CreatorName = dto.CreatorName;
            creator.CreatorAbout = dto.CreatorAbout;
            creator.CreatorCoverPath = coverfileName;
			creator.CreatorPicPath = picfileName;
           

            _appDbContext.SaveChanges();
            return NoContent();
        }

        [HttpPut]
        [Route("{creatorId}/songs/{songId}")]//修改創作者的單一歌曲
        public IActionResult EditSongByCreator([FromBody] CreatorUploadSongDTO data)
        {
            var songdata = data;
            var song = _appDbContext.Songs.FirstOrDefault(s => s.Id == songdata.Id);

            if (song == null)
            {
                return NotFound("歌曲不存在");
            }

            song.SongName = data.SongName!;
            song.GenreId = songdata.GenreId;
            //song.Duration = dto.Duration;
            song.IsInstrumental = songdata.IsInstrumental;
            song.Language = songdata.Language;
            song.IsExplicit = songdata.IsExplicit;
            song.Released = songdata.Released;
            song.SongWriter = songdata.SongWriter!;
            song.Lyric = songdata.Lyric;
            song.Status = songdata.Status;
            song.AlbumId = songdata.AlbumId == 0 ? null : songdata.AlbumId;

            //存檔
            _appDbContext.SaveChanges();
            return Ok("變更成功");
        }


        [HttpPut]
        [Route("{creatorId}/albums/{albumId}")]//修改創作者的單一專輯
        public IActionResult EdiAlbumByCreator([FromBody] CreateAlbumbyCreatorDTO data)
        {

            var album = _appDbContext.Albums.FirstOrDefault(s => s.Id == data.Id);

            if (album == null)
            {
                return NotFound("專輯不存在");
            }

            album.AlbumName = data.AlbumName;
            album.AlbumTypeId = data.AlbumTypeId;
            album.AlbumGenreId = data.AlbumGenreId;
            album.Released = data.Released;
            album.Description = data.Description;
            album.Songs.Clear();

            var songs = _appDbContext.Songs.Where(x => data.SongList.Contains(x.Id));
            foreach (var song in songs)
            {
                album.Songs.Add(song);
            }


            //存檔
            _appDbContext.SaveChanges();
            return Ok("變更成功");
        }

        [HttpPost]
        [Route("{creatorId}/deletealbum/{albumId}")]//刪除創作者的單一專輯
        public IActionResult DeleteAlbumByCreator(int creatorId, int albumId)
        {
            var album = _appDbContext.Albums
                .Include(a => a.AlbumType)
                .Include(a => a.AlbumGenre)
                .Include(a => a.MainCreator)
                .Where(a => a.MainCreatorId == creatorId && a.Id == albumId)
                .FirstOrDefault();

            if (album == null)
            {
                return NotFound();
            }

            _appDbContext.Albums.Remove(album);
            _appDbContext.SaveChanges();


            return Ok("成功刪除專輯");
        }

        [HttpPost]
        [Route("{creatorId}/song_creator_Metadata/{songId}")]//刪除創作者的單一歌曲
                                                             //[Route("deletesong")]//刪除創作者的單一歌曲
        public IActionResult DeleteSongByCreator(int songId, int creatorId)
        {
            //  int creatorId = 1;
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


            return Ok("成功刪除音樂");
        }
    }
}
