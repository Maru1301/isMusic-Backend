using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static api.iSMusic.Controllers.MembersController;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class PlaylistRepository: IRepository, IPlaylistRepository
	{
		private readonly AppDbContext _db;

		private readonly int skipNumber = 5;

		private readonly int takeNumber = 5;

		public PlaylistRepository(AppDbContext db)
		{
			_db = db;
		}

		public async Task CreatePlaylistAsync(PlaylistCreateVM newPlaylist)
		{
			_db.Playlists.Add(newPlaylist.ToEntity());
			await _db.SaveChangesAsync();
		}

		public IEnumerable<PlaylistIndexDTO> GetLikedPlaylists(int memberId)
		{
			return _db.LikedPlaylists
				.Where(lp => lp.MemberId == memberId)
				.Join(_db.Playlists, lp => lp.PlaylistId, p => p.Id, (lp, p) => new PlaylistIndexDTO
				{
					Id = p.Id,
					ListName = p.ListName,
					PlaylistCoverPath = p.PlaylistCoverPath,
					MemberId = p.MemberId,
				})
				.ToList();
		}

		public IEnumerable<PlaylistIndexDTO> GetMemberPlaylists(int memberId, InputQuery query)
		{
			bool includedLiked = query.IncludedLiked;

			var memberPlaylists = _db.Playlists
				.Where(playlist => includedLiked ?
				(playlist.MemberId == memberId || playlist.LikedPlaylists.Select(liked => liked.MemberId).Contains(memberId)) : (playlist.MemberId == memberId));
				

			switch (query.Condition)
			{
				case "Alphatically":
					memberPlaylists = memberPlaylists.OrderBy(playlist => playlist.ListName);
					break;
				case "RecentlyAdded":
					memberPlaylists = memberPlaylists.OrderBy(playlist => playlist.Created);
					break;
			}

			var dtos = memberPlaylists.Select(playlist => new PlaylistIndexDTO
			{
				Id= playlist.Id,
				PlaylistCoverPath = playlist.PlaylistCoverPath,
				ListName= playlist.ListName,
				MemberId= memberId,
			});

			return dtos;
		}

		public IEnumerable<PlaylistIndexDTO> GetMemberPlaylistsByName(int memberId, string name, int rowNumber)
		{
			var memberPlaylists = _db.Playlists
				.Where(playlist => playlist.MemberId == memberId && playlist.ListName.Contains(name))
				.Select(playlist => new PlaylistIndexDTO
				{
					Id = playlist.Id,
					ListName = playlist.ListName,
					PlaylistCoverPath = playlist.PlaylistCoverPath,
					MemberId = playlist.MemberId,
				})
				.Skip((rowNumber-1)*skipNumber)
				.Take(takeNumber)
				.ToList();

			return memberPlaylists;
		}

		public async Task<int> GetNumOfPlaylistsByMemberIdAsync(int memberId)
		{
			return await _db.Playlists.CountAsync(p => p.MemberId == memberId);
		}

		public Playlist? GetPlaylistByIdForCheck(int playlistId)
		{
			return _db.Playlists.Find(playlistId);
		}

		public PlaylistDetailDTO GetPlaylistById(int playlistId)
		{
			return _db.Playlists
				.Include(p => p.PlaylistSongMetadata)
				.Select(playlist => new PlaylistDetailDTO
				{
					Id = playlist.Id,
					ListName = playlist.ListName,
					IsPublic = playlist.IsPublic,
					MemberId = playlist.MemberId,
					PlaylistCoverPath = playlist.PlaylistCoverPath,
					PlayListSongMetadata = playlist.PlaylistSongMetadata
						.OrderBy(metadata => metadata.DisplayOrder)
						.Select(m => m.ToVM())
						.ToList()
				})
				.Single(p => p.Id == playlistId);
		}

		public IEnumerable<PlaylistIndexDTO> GetPlaylistsByName(string name, int skipRows, int takeRows)
		{
			return _db.Playlists
				.Where(playlist => playlist.ListName.Contains(name) && playlist.IsPublic)
				.Select(playlist => new PlaylistIndexDTO 
				{ 
					Id = playlist.Id,
					ListName= playlist.ListName,
					PlaylistCoverPath= playlist.PlaylistCoverPath,
					MemberId= playlist.MemberId,
					TotalLikes = playlist.LikedPlaylists.Count,
				})
				.OrderBy(dto => dto.TotalLikes)
				.Skip(skipRows)
				.Take(takeRows)
				.ToList();
		}

		public async Task<int> GetPlaylistIdByMemberIdAsync(int memberId)
		{
			return await _db.Playlists.Where(p => p.MemberId == memberId)
							  .OrderByDescending(p => p.Id)
							  .Select(p => p.Id)
							  .FirstAsync();
		}

		public IEnumerable<PlaylistIndexDTO> GetRecommended()
		{
			var recommendedPlaylists = _db.Playlists
				.Where(p => p.IsPublic)
				.Include(p => p.LikedPlaylists)
				.Select(p => new PlaylistIndexDTO
				{
					Id = p.Id,
					ListName = p.ListName,
					PlaylistCoverPath = p.PlaylistCoverPath,
					MemberId = p.MemberId,
					TotalLikes = p.LikedPlaylists.Count(),
				})
				.OrderByDescending(x => x.TotalLikes)
				.Take(10)
				.ToList();

			return recommendedPlaylists;
		}

		public void AddSongToPlaylist(int playlistId, int songId, int lastOrder)
		{
			var newMetadata = new PlaylistSongMetadatum
			{
				SongId = songId,
				PlayListId = playlistId,
				DisplayOrder = lastOrder + 1
			};

			_db.PlaylistSongMetadata.Add(newMetadata);
			_db.SaveChanges();
		}
	}
}
