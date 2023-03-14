using System.Buffers.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_practice.Models.ViewModels.PlaylistVMs
{
    public class PlaylistEditVM
    {
		[Required]
        public string ListName { get; set; } = null!;

		public IFormFile? PlaylistCover { get; set; }

        public string? Description { get; set; }
    }
}
