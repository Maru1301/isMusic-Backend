namespace api.iSMusic.Models.DTOs.MusicDTOs
{
    public class AlbumDTO
    {
		public int Id { get; set; }

		public string AlbumName { get; set; } = null!;

		public string AlbumCoverPath { get; set; } = null!;

		public int AlbumTypeId { get; set; }
				
		public int AlbumGenreId { get; set; }
		
		public DateTime Released { get; set; }

		public string Description { get; set; } = null!;

		public int? MainArtistId { get; set; }
		
		public int? MainCreatorId { get; set; }
		
		public string? AlbumProducer { get; set; }

		public string? AlbumCompany { get; set; }
	}
}