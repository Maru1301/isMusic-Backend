using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using API_practice.Models.ViewModels.PlaylistVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class PlaylistExts
{
    private static readonly string webUrl = "https://localhost:44373/Uploads/Covers/";

    public static PlaylistDetailVM ToDetailVM(this PlaylistDetailDTO source)
		=> new()
        {
			Id = source.Id,
			ListName = source.ListName,
			PlaylistCoverPath = webUrl + source.PlaylistCoverPath,
			MemberName = source.MemberName,
			MemberPicPath = webUrl + source.MemberPicPath,
			IsPublic = source.IsPublic,
			IsLiked = source.IsLiked,
			IsOwner = source.IsOwner,
			TotalLikes = source.TotalLikes,
			Metadata = source.Metadata.Select(metadata => metadata.ToVM()).ToList(),
		};

	public static PlaylistIndexVM ToIndexVM(this PlaylistIndexDTO source)
		=> new()
		{
			Id= source.Id,
			ListName= source.ListName,
			MemberId= source.MemberId,
			PlaylistCoverPath = source.PlaylistCoverPath != null
				? webUrl + source.PlaylistCoverPath
				: "",
			TotalLikes = source.TotalLikes,
			IsLiked= source.IsLiked,
			OwnerName = source.OwnerName,
		};

	public static Playlist ToEntity(this PlaylistCreateVM source)
		=> new()
        {
			ListName = source.ListName,
			MemberId = source.MemberId,
			Created = DateTime.Now,
		};

	public static PlaylistSongMetadataVM ToVM(this PlaylistSongMetadatum source)
		=> new()
        {
			Id= source.Id,
			PlayListId = source.PlayListId,
			SongId = source.SongId,
			DisplayOrder= source.DisplayOrder,
			AddedTime= source.AddedTime,
		};

	public static PlaylistEditDTO ToEditDTO(this PlaylistEditVM source)
		=> new()
        {
			ListName= source.ListName,
			PlaylistCover = source.PlaylistCover,
			Description= source.Description,
		};

	public static PlaylistSongMetadataDTO ToDTO(this PlaylistSongMetadatum source)
		=> new()
		{
			Id = source.Id,
			PlayListId= source.PlayListId,
			DisplayOrder = source.DisplayOrder,
			AddedTime = source.AddedTime,
			Song = source.Song.ToInfoDTO(),
		};

	public static PlaylistSongMetadataVM ToVM(this PlaylistSongMetadataDTO source) => new()
	{
		Id = source.Id,
		PlayListId = source.PlayListId,
		DisplayOrder = source.DisplayOrder,
		AddedTime = source.AddedTime,
		Song = source.Song.ToInfoVM(),
	};
}