using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.ArtistVMs;
using API_practice.Models.ViewModels.CreatorVMs;
using API_practice.Models.ViewModels.SongVMs;

namespace API_practice.Models.ViewModels.SongVMs
{
	public class SongIndexVM
	{
		public int Id { get; set; }

		public string SongName { get; set; } = null!;

		public string GenreName { get; set; } = null!;

		public bool? IsExplicit { get; set; }

		public string SongCoverPath { get; set; } = null!;

		public string SongPath { get; set; } = null!;

		public int AlbumId { get; set; }

		public int PlayedTimes { get; set; }

		public List<ArtistInfoVM> Artistlist { get; set; } = new List<ArtistInfoVM>();

		public List<CreatorInfoVM> Creatorlist { get; set; } = new List<CreatorInfoVM>();

	}
}