﻿using API_practice.Models.EFModels;

namespace API_practice.Models.ViewModels.SongVMs
{
	public class SongInfoVM
	{
		public int Id { get; set; }

		public string SongName { get; set; } = null!;

		public int Duration { get; set; }

		public bool? IsExplicit { get; set; }

		public DateTime Released { get; set; }

		public string SongCoverPath { get; set; } = null!;

		public string SongPath { get; set; } = null!;

		public bool Status { get; set; }

		public int? AlbumId { get; set; }

		public string AlbumName { get; set; } = null!;

		public bool IsLiked { get; set; }
	}
}
