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

		public QueuesController(IQueueRepository repository, ISongRepository songRepository, IArtistRepository artistRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
			_repository = repository;
			_songRepository = songRepository;
			_service = new(_repository, songRepository, artistRepository, albumRepository, playlistRepository);
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
		public IActionResult ChangeQueueContent(int queueId, int contentId, [FromBody] Condition condition)
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

		public class Condition
		{
			public bool SingleSong { get; set; }

			public bool Artist { get; set; }

			public bool Album { get; set; }

			public bool Playlist { get; set; }
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
		[Route("{queueId}/NextSong")]
		public IActionResult NextSong(int queueId)
		{
			var result = _service.NextSong(queueId);
			if(!result.Success)
			{
				return NotFound(result.Message);
			}
			else if(result.Dto == null)
			{
				return Accepted(result.Message);
			}

            return Ok(result.Dto);
		}

        [HttpPut]
        [Route("{queueId}/Previous")]
        public IActionResult PreviousSong(int queueId)
        {
            var result = _service.PreviousSong(queueId);
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
		[Route("{queueId}/Shuffle")]
		public IActionResult ChangeShuffle(int queueId)
		{
			var result = _service.ChangeShuffle(queueId);
			if(!result.Success)
				return NotFound(result.Message);

			return Ok(result.Message);
		}

		[HttpPatch]
		[Route("{queueId}/Repeat")]
		public IActionResult ChangeRepeat(int queueId, [FromQuery]string mode)
		{
			var result = _service.ChangeRepeat(queueId, mode);
			if (!result.Success)
				return NotFound(result.Message);

			var queue = _repository.GetQueueById(queueId);
			if (queue == null)
				return NotFound("佇列不存在");

			return Ok(queue.ToIndexVM());
		}
	}
}