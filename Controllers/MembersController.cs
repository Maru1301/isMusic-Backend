using api.iSMusic.Models;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class MembersController : ControllerBase
	{
		private readonly AppDbContext _db;

		public MembersController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		[Route("{memberId}/Playlists")]
		public ActionResult<IEnumerable<PlaylistIndexVM>> GetMemberPlaylist([FromRoute]int memberId, [FromBody]bool myOwn)
		{
			var data = _db.Playlists;

			var member = _db.Members.Single(member => member.Id == memberId);

			if(member == null)
			{
				return NotFound("Member not found");
			}

			if (memberId > 0)
			{
				data.Where(p => p.MemberId == memberId);
			}

			return Ok(data.ToList().Select(p => p.ToIndexVM()));
		}
	}
}
