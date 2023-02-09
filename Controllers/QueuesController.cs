using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.QueueVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class QueuesController : ControllerBase
	{
		private readonly AppDbContext _db;
		public QueuesController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet(Name = "GetAll")]
		public ActionResult<IEnumerable<Queue>> GetAll()
		{
			var data = _db.Queues.ToList();

			return Ok(data);
		}

		[HttpPost]
		[Route("{memberId}")]
		//todo edit the method
		public void CreateQueue(int memberId)
		{
			var queue = new Queue()
			{
				MemberId = memberId,
				CurrentSongId = null,
				CurrentSongTime = null,
				IsShuffle = false,
				IsRepeat = null,
			};
			_db.Queues.Add(queue);
			_db.SaveChanges();
		}

		[HttpPost]
		[Route("Song")]
		public ActionResult AddSongIntoQueue([FromBody] SongQueueInput input)
		{
			var queue = _db.Queues.SingleOrDefault(q => q.Id == input.QueueId);

			if (queue == null)
			{
				return NotFound(new { message = "Queue not found" });
			}

			var song = _db.Songs.SingleOrDefault(s => s.Id == input.SongId);

			if (song == null)
			{
				return NotFound(new { message = "Song not found" });
			}

			var album = _db.Albums.SingleOrDefault(a => a.Id == input.AlbumId);

			if (album == null)
			{
				return NotFound(new { message = "Album not found" });
			}

			var playlist = _db.Playlists.SingleOrDefault(p => p.Id == input.PlaylistId);

			if (playlist == null)
			{
				return NotFound(new { message = "Playlist not found" });
			}

			QueueSong metadata = new QueueSong
			{
				SongId = input.SongId,
				QueueId = input.QueueId
			};

			var lastSong = _db.QueueSongs.Where(qs => qs.QueueId == input.QueueId)
										  .OrderByDescending(qs => qs.DisplayOrder)
										  .FirstOrDefault();

			if (lastSong == null)
			{
				metadata.DisplayOrder = 1;
			}
			else
			{
				metadata.DisplayOrder = lastSong.DisplayOrder + 1;
			}

			_db.QueueSongs.Add(metadata);
			_db.SaveChanges();

			return Ok(new { message = "Song added to the queue successfully" });
		}

		public class SongQueueInput
		{
			public int SongId { get; set; }

			public int QueueId { get; set; }

			public int? AlbumId { get; set; }

			public int? PlaylistId { get; set; }
		}

		[HttpDelete]
		[Route("Song")]
		public ActionResult<Song> DeleteSongFromQueue(int queueId)
		{
			var queue = _db.Queues.SingleOrDefault(q => q.Id == queueId);

			if (queue == null)
			{
				return NotFound();
			}

			var song = _db.Songs.Where(song => song.Id == queue.CurrentSongId).Single();

			var metadata = _db.QueueSongs.Where(q => q.SongId == queue.CurrentSongId && q.QueueId == queue.Id).Single();
			_db.Entry(metadata).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
			_db.SaveChanges();

			return song;
		}

		[HttpPatch]
		[Route("Member/{memberId}/shuffle")]
		public void ChangeShuffle(int memberId, [FromBody] bool isShuffle)
		{
			Queue queue = _db.Queues.Where(queue => queue.MemberId == memberId).Single();

			queue.IsShuffle = isShuffle;

			_db.Entry(queue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			_db.SaveChanges();
		}

		[HttpPatch]
		[Route("Member/{memberId}/repeat")]
		public void ChangeRepeat(int memberId, [FromBody] bool? isRepeat)
		{
			Queue queue = _db.Queues.Where(queue => queue.MemberId == memberId).Single();

			queue.IsRepeat = isRepeat;

			_db.Entry(queue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			_db.SaveChanges();
		}
	}
}