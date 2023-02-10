using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
	public class PlaylistRepository: IRepository, IPlaylistRepository
	{
		private readonly AppDbContext _db;

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

		public IEnumerable<PlaylistIndexDTO> GetMemberPlaylists(int memberId)
		{
			var memberPlaylists = _db.Playlists
				.Where(playlist => playlist.MemberId == memberId)
				.Select(playlist => new PlaylistIndexDTO
				{
					Id = playlist.Id,
					ListName = playlist.ListName,
					PlaylistCoverPath = playlist.PlaylistCoverPath,
					MemberId = playlist.MemberId,
				})
				.ToList();

			return memberPlaylists;
		}

		public async Task<int> GetNumOfPlaylistsByMemberIdAsync(int memberId)
		{
			return await _db.Playlists.CountAsync(p => p.MemberId == memberId);
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
					PlayListSongMetadata = playlist.PlaylistSongMetadata.Select(m => m.ToVM()).ToList()
				})
				.Single(p => p.Id == playlistId);
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
	}
}
