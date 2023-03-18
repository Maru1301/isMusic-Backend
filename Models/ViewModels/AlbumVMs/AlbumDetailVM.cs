using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.ViewModels.SongVMs;
using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.ViewModels.AlbumVMs
{
	public class AlbumDetailVM
	{
		public int Id { get; set; }

		public string AlbumName { get; set; } = null!;

		public string AlbumCoverPath { get; set; } = null!;

		public int AlbumTypeId { get; set; }

		public string? AlbumTypeName { get; set; }

		public int AlbumGenreId { get; set; }

		public string? AlbumGenreName { get; set; }

		[DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
		public DateTime Released { get; set; }

        public bool IsLiked { get; set; }

		public string Description { get; set; } = null!;

		public int? MainArtistId { get; set; }

		public string? MainArtistName { get; set; }

        public string? MainArtistPicPath { get; set; }

        public int? MainCreatorId { get; set; }

		public string? MainCreatorName { get; set; }

        public string? MainCreatorPicPath { get; set; }

        public string? AlbumProducer { get; set; }

		public string? AlbumCompany { get; set; }

		public List<SongInfoVM> Songs { get; set; } = new();
	}
}
