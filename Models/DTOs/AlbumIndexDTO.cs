namespace api.iSMusic.Models.DTOs
{
	public class AlbumIndexDTO
	{
		public int Id { get; set; }

		public string AlbumName { get; set; } = null!;

		public string AlbumCoverPath { get; set; } = null!;

		public int AlbumTypeId { get; set; }

		public int AlbumGenreId { get; set; }

		public DateTime Released { get; set; }

		public int MainArtistId { get; set; }

		public int TotalLikes { get; set; }
	}
}
