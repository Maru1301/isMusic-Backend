using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.ArtistVMs;
using api.iSMusic.Models.ViewModels.CreatorVMs;

namespace api.iSMusic.Models.ViewModels.SongVMs
{
	public class SongInfoVM
	{
		public int Id { get; set; }

		public string SongName { get; set; } = null!;

		public int Duration { get; set; }

		public bool? IsExplicit { get; set; }

        public bool FromList { get; set; }

        public DateTime Released { get; set; }

		public string SongCoverPath { get; set; } = null!;

		public string SongPath { get; set; } = null!;

		public bool Status { get; set; }

		public int? AlbumId { get; set; }

		public string AlbumName { get; set; } = null!;

		public bool IsLiked { get; set; }

        public int? DisplayOrderInAlbum { get; set; }

        public int PlayedTimes { get; set; }

		public List<ArtistInfoVM> Artists { get; set; } = new();

        public List<CreatorInfoVM> Creators { get; set; } = new();
    }
}
