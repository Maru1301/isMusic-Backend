using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.ViewModels.SongVMs
{
	public class SongIndexVM
	{
		public int Id { get; set; }

		public string SongName { get; set; } = null!;

		public string GenreName { get; set; } = null!;

        public int Duration { get; set; }

        public bool? IsExplicit { get; set; }

        public bool IsLiked { get; set; }

        public string SongCoverPath { get; set; } = null!;

		public string SongPath { get; set; } = null!;

		public int AlbumId { get; set; }

		public int PlayedTimes { get; set; }

		public IEnumerable<ArtistInfoVM> Artistlist { get; set; } = new List<ArtistInfoVM>();

		public IEnumerable<CreatorInfoVM> Creatorlist { get; set; } = new List<CreatorInfoVM>();

	}
}