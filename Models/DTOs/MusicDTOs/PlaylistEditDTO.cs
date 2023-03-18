using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
	public class PlaylistEditDTO
	{
		public string ListName { get; set; } = null!;

		public IFormFile? PlaylistCover { get; set; }

		public string? PlaylistCoverPath { get; set; }

		public string? Description { get; set; }
	}
}
