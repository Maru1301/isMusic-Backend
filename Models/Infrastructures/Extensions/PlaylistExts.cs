using API_practice.Models.EFModels;
using API_practice.Models.ViewModels.PlaylistVMs;

public static class PlaylistExts
{
	public static PlaylistDetailVM ToDetailVM(this Playlist source)
		=> new PlaylistDetailVM
		{
			Id = source.Id,
			ListName = source.ListName,
			PlaylistCoverPath = source.PlaylistCoverPath,
			MemberAccount = source.MemberAccount,
			IsPublic= source.IsPublic,
			PlayListSongMetadata = source.PlayListSongMetadata.Select(m=>m.ToVM()).ToList(),
		};

	public static PlaylistIndexVM ToIndexVM(this Playlist source)
		=> new PlaylistIndexVM
		{
			Id = source.Id,
			ListName = source.ListName,
			PlaylistCoverPath = source.PlaylistCoverPath,
			MemberAccount = source.MemberAccount,
		};

	public static Playlist ToEntity(this PlaylistCreateVM source)
		=> new Playlist
		{
			ListName = source.ListName,
			MemberAccount = source.MemberAccount,
		};
}