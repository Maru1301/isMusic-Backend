using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
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

		private readonly QueueService _service;

		public QueuesController(IQueueRepository repository, ISongRepository songRepository, IArtistRepository artistRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
			_repository = repository;
			_service = new(_repository, songRepository, artistRepository, albumRepository, playlistRepository);
		}

		//[HttpPost]
		//[Route("{memberId}")]
		////todo edit the method
		//public void CreateQueue(int memberId)
		//{
		//	var queue = new Queue()
		//	{
		//		MemberId = memberId,
		//		CurrentSongId = null,
		//		CurrentSongTime = null,
		//		IsShuffle = false,
		//		IsRepeat = null,
		//	};
		//	_db.Queues.Add(queue);
		//	_db.SaveChanges();
		//}

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
		[Route("{queueId}/Songs/{songId}")]
		public IActionResult UpdateByQueueSong(int queueId, int songId)
		{
			var result = _service.UpdateByQueueSong(queueId, songId);

			if(!result.Success)
			{
				return BadRequest(result.Message);
			}

			var updatedQueue = _repository.GetQueueById(queueId);

			if (updatedQueue == null)
			{
				return NoContent();
			}

			return Ok(updatedQueue.ToIndexVM());
		}

		//[HttpDelete]
		//[Route("Song")]
		//public ActionResult<Song> DeleteSongFromQueue(int queueId)
		//{
		//	var queue = _db.Queues.SingleOrDefault(q => q.Id == queueId);

		//	if (queue == null)
		//	{
		//		return NotFound();
		//	}

		//	var song = _db.Songs.Where(song => song.Id == queue.CurrentSongId).Single();

		//	var metadata = _db.QueueSongs.Where(q => q.SongId == queue.CurrentSongId && q.QueueId == queue.Id).Single();
		//	_db.Entry(metadata).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
		//	_db.SaveChanges();

		//	return song;
		//}

		//[HttpPatch]
		//[Route("Member/{memberId}/shuffle")]
		//public void ChangeShuffle(int memberId, [FromBody] bool isShuffle)
		//{
		//	Queue queue = _db.Queues.Where(queue => queue.MemberId == memberId).Single();

		//	queue.IsShuffle = isShuffle;

		//	_db.Entry(queue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
		//	_db.SaveChanges();
		//}

		//[HttpPatch]
		//[Route("Member/{memberId}/repeat")]
		//public void ChangeRepeat(int memberId, [FromBody] bool? isRepeat)
		//{
		//	Queue queue = _db.Queues.Where(queue => queue.MemberId == memberId).Single();

		//	queue.IsRepeat = isRepeat;

		//	_db.Entry(queue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
		//	_db.SaveChanges();
		//}
	}
}