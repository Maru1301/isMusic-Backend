using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using API_practice.Models.ViewModels.PlaylistVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions;

public static class PlaylistExts
{
	public static PlaylistDetailVM ToDetailVM(this PlaylistDetailDTO source)
		=> new PlaylistDetailVM
		{
			Id = source.Id,
			ListName = source.ListName,
			PlaylistCoverPath = source.PlaylistCoverPath,
			MemberId = source.MemberId,
			IsPublic = source.IsPublic,
			PlayListSongMetadata = source.PlayListSongMetadata,
		};

	public static PlaylistIndexVM ToIndexVM(this Playlist source)
		=> new PlaylistIndexVM
		{
			Id = source.Id,
			ListName = source.ListName,
			MemberId= source.MemberId,
			PlaylistCoverPath = source.PlaylistCoverPath,
		};

	public static PlaylistIndexVM ToIndexVM(this PlaylistIndexDTO source)
		=> new()
		{
			Id= source.Id,
			ListName= source.ListName,
			MemberId= source.MemberId,
			PlaylistCoverPath = source.PlaylistCoverPath,
			TotalLikes = source.TotalLikes,
		};

	public static Playlist ToEntity(this PlaylistCreateVM source)
		=> new Playlist
		{
			ListName = source.ListName,
			MemberId = source.MemberId,
			Created = DateTime.Now,
		};

	public static PlaylistSongMetadataVM ToVM(this PlaylistSongMetadatum source)
		=> new PlaylistSongMetadataVM
		{
			Id= source.Id,
			PlayListId = source.PlayListId,
			SongId = source.SongId,
			DisplayOrder= source.DisplayOrder,
			AddedTime= source.AddedTime,
		};

	public static PlaylistEditDTO ToEditDTO(this PlaylistEditVM source)
		=> new PlaylistEditDTO
		{
			ListName= source.ListName,
			PlaylistCover = source.PlaylistCover,
			PlaylistCoverPath= source.PlaylistCoverPath,
			Description= source.Description,
		};
}