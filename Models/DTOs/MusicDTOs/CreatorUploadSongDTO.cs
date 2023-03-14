using api.iSMusic.Models.EFModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.DTOs.MusicDTOs
{
	public class CreatorUploadSongDTO
	{
		
		public int? Id { get; set; }
		public string SongName { get; set; } = null!;
		public int GenreId { get; set; }
		public string? GenreName { get; set; }

		public int Duration { get; set; }
		public bool IsInstrumental { get; set; }
		public string? Language { get; set; }
		public bool? IsExplicit { get; set; }
		public DateTime Released { get; set; }
		public string SongWriter { get; set; } = null!;
		public string? Lyric { get; set; }
		public string? SongCoverPath { get; set; } 
		public string? SongPath { get; set; } 
		public bool Status { get; set; }
		public int? AlbumId { get; set; }
		public IFormFile? Song { get; set; }
		public IFormFile? Cover { get; set; }
	}
}
