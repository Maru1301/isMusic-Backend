using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.ViewModels.AlbumVMs
{
	public class AlbumIndexVM
	{
		public int Id { get; set; }

		public string AlbumName { get; set; } = null!;

		public string AlbumCoverPath { get; set; } = null!;

		public int AlbumTypeId { get; set; }

		public int AlbumGenreId { get; set; }

		public DateTime Released { get; set; }

		public int? MainArtistId { get; set; }

		public string? MainArtistName { get; set; }

        public string? MainArtistPicPath { get; set; }

        public int? MainCreatorId { get; set; }

		public string? MainCreatorName { get; set; }

        public string? MainCreatorPicPath{ get; set; }

        public int TotalLikes { get; set; }

        public bool IsLiked { get; set; }
    }
}
