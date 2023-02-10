using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.SongVMs;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
	public class SongRepository : IRepository, ISongRepository
	{
		private readonly AppDbContext _db = new AppDbContext();

		public SongRepository(AppDbContext db)
		{
			_db = db;
		}

		public List<int> GetLikedSongIdsByMemberId(int memberId)
		{
			return _db.LikedSongs
				.Where(l => l.MemberId == memberId)
				.Select(l => l.SongId)
				.ToList();
		}

		public IEnumerable<SongIndexDTO> GetPopularSongs()
		{
			var data = _db.Songs
				.Where(s => s.AlbumId != null && s.Status != false)
				.OrderByDescending(s => s.SongPlayedRecords.Count())
				.Take(10)
				.Select(s => new SongIndexDTO
				{
					Id = s.Id,
					SongName = s.SongName,
					GenreName = s.Genre.GenreName,
					IsExplicit = s.IsExplicit,
					SongCoverPath = s.SongCoverPath,
					SongPath = s.SongPath,
					AlbumId = s.AlbumId != null ? s.AlbumId.Value : 0,
					PlayedTimes = s.SongPlayedRecords.Count(),
					Artistlist = s.SongArtistMetadata.Select(m => m.Artist.ToInfoVM()).ToList(),
					Creatorlist = s.SongCreatorMetadata.Select(m => m.Creator.ToInfoVM()).ToList(),
				});

			return data;
		}

		public IEnumerable<SongGenreInfo> GetSongGenres()
		{
			return _db.SongGenres.Select(songGenre => new SongGenreInfo
			{
				Id = songGenre.Id,
				GenreName = songGenre.GenreName
			});
		}

		public List<SongInfoDTO> GetSongsByPlaylistId(int playlistId)
		{
			return _db.Songs
				.Include(s => s.Album)
				.Where(s => s.PlaylistSongMetadata.Select(m => playlistId).Contains(playlistId))
				.Select(song => new SongInfoDTO
				{
					Id = song.Id,
					SongName = song.SongName,
					SongCoverPath
					= song.SongCoverPath,
					SongPath= song.SongPath,
					AlbumId = song.AlbumId,
					Status= song.Status,
					AlbumName = song.Album != null ? song.Album.AlbumName : string.Empty,
					Duration = song.Duration,
					IsExplicit= song.IsExplicit,
					Released = song.Released,
				})
				.ToList();
		}

		public IEnumerable<SongIndexDTO> SearchBySongName(string songName)
		{
			return _db.Songs.Where(song => song.SongName.Contains(songName) && song.Status == true && song.AlbumId != null).Select(song => new SongIndexDTO
			{
				Id = song.Id,
				SongName = song.SongName,
				SongCoverPath = song.SongCoverPath,
				SongPath = song.SongPath,
				AlbumId = song.AlbumId ?? 0,
				Artistlist = song.SongArtistMetadata.Select(metadata => metadata.Artist).Select(artist => artist.ToInfoVM()),
				Creatorlist = song.SongCreatorMetadata.Select(metadata => metadata.Creator).Select(creator => creator.ToInfoVM()),
				GenreName = song.Genre.GenreName,
				IsExplicit= song.IsExplicit,
				PlayedTimes = song.SongPlayedRecords.Count(),
			});
		}
	}
}
