namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class AlbumDTO
    {
		public int Id { get; set; }

		public string AlbumName { get; set; } = null!;

		public string AlbumCoverPath { get; set; } = null!;

		public int AlbumTypeId { get; set; }
		public string? AlbumTypeName { get; set; }

		public int AlbumGenreId { get; set; }
		public string? AlbumGenreName { get; set; }

		public DateTime Released { get; set; }

		public string Description { get; set; } = null!;

		public int? MainArtistId { get; set; }
		
		public int? MainCreatorId { get; set; }
		public string? MainCreatorName { get; set; }

		public string? AlbumProducer { get; set; }

		public string? AlbumCompany { get; set; }
		public string? Cover { get; set; }
		public int[]? SongList { get; set; }

    }
}