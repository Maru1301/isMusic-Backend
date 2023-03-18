using api.iSMusic.Models.EFModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.ViewModels.SongVMs
{
	public class SongGenreInfo
	{
		public int Id { get; set; }

		public string GenreName { get; set; } = null!;
	}
}
