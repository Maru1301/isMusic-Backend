using api.iSMusic.Models;
using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.QueueVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class QueuesController : ControllerBase
	{
		private readonly IQueueRepository _repository;

		private readonly ISongRepository _songRepository;

		private readonly QueueService _service;

		public QueuesController(IQueueRepository repository, ISongRepository songRepository, IArtistRepository artistRepository, ICreatorRepository creatorRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
			_repository = repository;
			_songRepository = songRepository;
			_service = new(_repository, songRepository, artistRepository, creatorRepository, albumRepository, playlistRepository);
        }

		private int GetMemberId()
		{
            return Int32.Parse(HttpContext.User.Claims.First(claim => claim.Type == "MemberId").Value);
        }

		[HttpPost]
		[Route("{queueId}/Songs/{songId}")]
		public IActionResult AddSongIntoQueue(int queueId, int songId)
		{
			var result = _service.AddSongIntoQueue(queueId, songId);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			var updatedQueue = _repository.GetQueueById(queueId);

			if (updatedQueue == null)
			{
				return NoContent();
			}

			updatedQueue = ProcessLikedSongs(updatedQueue);

			return Ok(updatedQueue.ToIndexVM());
		}

		[HttpPost]
		[Route("{queueId}/Playlists/{playlistId}")]
		public IActionResult AddPlaylistIntoQueue(int queueId, int playlistId)
		{
			var result = _service.AddPlaylistIntoQueue(queueId, playlistId);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			var updatedQueue = _repository.GetQueueById(queueId);

			if (updatedQueue == null)
			{
				return NoContent();
			}

			updatedQueue = ProcessLikedSongs(updatedQueue);

			return Ok(updatedQueue.ToIndexVM());
		}

		[HttpPost]
		[Route("{queueId}/Albums/{albumId}")]
		public IActionResult AddAlbumIntoQueue(int queueId, int albumId)
		{
			var result = _service.AddAlbumIntoQueue(queueId, albumId);
			if (!result.Success)
			{
				return BadRequest(result.Message);
			}

			var updatedQueue = _repository.GetQueueById(queueId);

			if (updatedQueue == null)
			{
				return NoContent();
			}

			updatedQueue = ProcessLikedSongs(updatedQueue);

			return Ok(updatedQueue.ToIndexVM());
		}

		private QueueIndexDTO ProcessLikedSongs(QueueIndexDTO updatedQueue)
		{
			var queueSongIds = updatedQueue.SongInfos.Select(info => info.Id);

			var memberId = updatedQueue.MemberId;

			var likedSongIds = _songRepository.GetLikedSongIdsByMemberId(memberId);

			foreach (int queueSongId in queueSongIds)
			{
				if (likedSongIds.Contains(queueSongId))
				{
					updatedQueue.SongInfos.Single(info => info.Id == queueSongId).IsLiked = true;
				}
			}

			return updatedQueue;
		}

		[HttpPut]
		[Route("{queueId}/{contentId}")]
		public IActionResult ChangeQueueContent(int queueId, int contentId, [FromBody]string condition)
		{
			var result = _service.ChangeQueueContent(queueId, contentId, condition);

			if (!result.Success)
			{
				return NotFound(new { message = result.Message });
			}

			var updatedQueue = _repository.GetQueueById(queueId);

			if (updatedQueue == null)
			{
				return NoContent();
			}

			updatedQueue = ProcessLikedSongs(updatedQueue);

			return Ok(updatedQueue.ToIndexVM());
		}

		[HttpPut]
		[Route("{queueId}/DisplayOredr/{displayOrder}")]
		public IActionResult UpdateByDisplayOredr(int queueId, int displayOrder)
		{
			var result = _service.UpdateByDisplayOredr(queueId, displayOrder);

			if(!result.Success)
			{
				return BadRequest(result.Message);
			}

			var updatedQueue = _repository.GetQueueById(queueId);

			if (updatedQueue == null)
			{
				return NoContent();
			}

			updatedQueue = ProcessLikedSongs(updatedQueue);

			return Ok(updatedQueue.ToIndexVM());
		}

		[HttpPut]
		[Route("NextSong")]
		public IActionResult NextSong()
		{
			int memberId = GetMemberId();
            var result = _service.NextSong(memberId);
			if(!result.Success)
			{
				return NotFound(result.Message);
			}
			else if(result.Dto == null)
			{
				return Ok(result.Message);
			}

            return Ok(result.Dto);
		}

        [HttpPut]
        [Route("/Previous")]
        public IActionResult PreviousSong()
        {
            int memberId = GetMemberId();
            var result = _service.PreviousSong(memberId);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            else if (result.Dto == null)
            {
                return Accepted(result.Message);
            }

            return Ok(result.Dto);
        }

        [HttpPatch]
		[Route("ShuffleSetting")]
		public IActionResult ChangeShuffle()
		{
            int memberId = GetMemberId();
            var result = _service.ChangeShuffle(memberId);
			if(!result.Success)
				return NotFound(result.Message);

			return Ok(result.Message);
		}

        [HttpPatch]
        [Route("RepeatSetting")]
        public async Task<IActionResult> ChangeRepeat([FromQuery] string mode)
        {
            int memberId = GetMemberId();
            var result = _service.ChangeRepeat(memberId, mode);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            var queue = await _repository.GetQueueByMemberIdAsync(memberId);
            if (queue == null)
            {
                return NotFound("佇列不存在");
            }

            return Ok(queue.ToIndexVM());
        }
    }
}