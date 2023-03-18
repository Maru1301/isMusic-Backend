namespace api.iSMusic.Models.ViewModels.PlaylistVMs
{
	public class PlaylistIndexVM
	{
		public int Id { get; set; }

		public string ListName { get; set; } = null!;

		public string? PlaylistCoverPath { get; set; }

		public int MemberId { get; set; }

        public string? OwnerName{ get; set; }

        public int TotalLikes { get; set; }

        public bool IsLiked { get; set; }
    }
}
