using api.iSMusic.Models.ViewModels.CreatorVMs;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.ViewModels.AlbumVMs
{
	public class CreatorAlbumVM
	{
		//public int Id { get;  set; }
		public string? AlbumName { get;  set; }
		public string? AlbumCoverPath { get; set; }
		public DateTime Released { get; set; }
		public string? Description { get; internal set; }
		public string? AlbumProducer { get; internal set; }
		public string? AlbumCompany { get; internal set; }
		public string? AlbumTypeName { get; internal set; }
		public string? AlbumGenreName { get; internal set; }
		public List<CreatorSongVM>? Songs { get; set; }=new List<CreatorSongVM>();
	}
}