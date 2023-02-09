using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.QueueVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace practice.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class QueueController : ControllerBase
	{
		private readonly AppDbContext _db;
		public QueueController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet(Name ="GetAll")]
		public ActionResult<IEnumerable<Queue>> GetAll()
		{
			var data = _db.Queues.ToList();

			return Ok(data);
		}

		[HttpGet()]
		[Route("{memberAccount}")]
		public ActionResult<QueueIndexVM> Get(string memberAccount)
		{
			var data = _db.Queues.Include(q=>q.CurrentSong).Include(q=>q.QueueSongs).Where(q=>q.MemberAccount == memberAccount).Single();

			return Ok(data);
		}

		[HttpPost]
		[Route("{memberAccount}")]
		public void CreateQueue(string memberAccount)
		{
			var queue = new Queue()
			{
				MemberAccount = memberAccount,
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
		public void Push([FromBody]int queueId, int songId)
		{
			var queue = _db.Queues.Single(q => q.Id == queueId);

			QueueSong metadata = new QueueSong();
			metadata.SongId = songId;
			metadata.QueueId = queueId;
			metadata.NextQueueSong = null;
			metadata.FromAlbumOrPlaylist = false;

			_db.QueueSongs.Add(metadata);
			_db.SaveChanges();

			// if there is no song in the queue
			if (queue.CurrentSongId != null)
			{
				var lastSong = _db.QueueSongs.Single(qs => qs.NextQueueSong == null && qs.QueueId == queueId);

				// find added metadata id
				var addedMetadata = _db.QueueSongs.Single(qs => qs.QueueId == queueId && qs.SongId == songId && qs.Id != lastSong.Id);

				lastSong.NextQueueSong = addedMetadata.Id;
			}
		}

		[HttpDelete]
		[Route("Song")]
		public ActionResult<Song> Pop(int queueId)
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
		[Route("Member/{memberAccount}/shuffle")]
		public void ChangeShuffle(string memberAccount, [FromBody] bool isShuffle)
		{
			Queue queue = _db.Queues.Where(queue => queue.MemberAccount == memberAccount).Single();

			queue.IsShuffle = isShuffle;

			_db.Entry(queue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			_db.SaveChanges();
		}

		[HttpPatch]
		[Route("Member/{memberAccount}/repeat")]
		public void ChangeRepeat(string memberAccount, [FromBody] bool? isRepeat)
		{
			Queue queue = _db.Queues.Where(queue => queue.MemberAccount == memberAccount).Single();

			queue.IsRepeat = isRepeat;

			_db.Entry(queue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			_db.SaveChanges();
		}
	}
}