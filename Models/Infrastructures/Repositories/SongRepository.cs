using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.SongVMs;
using Microsoft.EntityFrameworkCore;

namespace api.iSMusic.Models.Infrastructures.Repositories
{
    public class SongRepository : IRepository, ISongRepository
	{
		private readonly AppDbContext _db;

		public SongRepository(AppDbContext db)
		{
			_db = db;
		}

		public IEnumerable<int> GetLikedSongIdsByMemberId(int memberId)
		{
			return _db.LikedSongs
				.Where(l => l.MemberId == memberId)
				.Select(l => l.SongId)
				.ToList();
		}

		public IEnumerable<SongIndexDTO> GetPopularSongs(int artistId = 0)
		{
			var songs = _db.Songs.Where(s => s.AlbumId != null && s.Status != false);

			if(artistId != 0)
			{
				songs = songs.Where(song => song.SongArtistMetadata.Select(metadata => metadata.ArtistId).Contains(artistId));
			}

			var dtos = songs
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
				})
				.ToList();

			return dtos;
		}

		public IEnumerable<SongGenreInfo> GetSongGenres()
		{
			return _db.SongGenres.Select(songGenre => new SongGenreInfo
			{
				Id = songGenre.Id,
				GenreName = songGenre.GenreName
			});
		}

		public IEnumerable<SongInfoDTO> GetSongsByPlaylistId(int playlistId)
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

		public IEnumerable<SongIndexDTO> GetSongsByName(string songName, int skipRows, int takeRows)
		{
			return _db.Songs
				.Where(song => song.SongName.Contains(songName) && song.Status == true && song.AlbumId != null)
				.Select(song => new SongIndexDTO
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
				})
				.OrderBy(dto => dto.PlayedTimes)
				.Skip(skipRows)
				.Take(takeRows)
				.ToList();
		}

		public Song? GetSongByIdForCheck(int songId)
		{
			return _db.Songs.Find(songId);
		}

		public SongIndexDTO? GetSongById(int id)
		{
			return _db.Songs
				.Select(song => new SongIndexDTO
				{
					Id = song.Id,
					SongName = song.SongName,
					GenreName = song.Genre.GenreName,
					IsExplicit = song.IsExplicit,
					SongCoverPath = song.SongCoverPath,
					SongPath = song.SongPath,
					AlbumId = song.AlbumId ?? 0,
					PlayedTimes= song.SongPlayedRecords.Count(),
					Artistlist = song.SongArtistMetadata.Select(metadata => metadata.Artist).Select(artist => artist.ToInfoVM()),
					Creatorlist = song.SongCreatorMetadata.Select(metadata => metadata.Creator).Select(creator => creator.ToInfoVM()),
					SongWriter = song.SongWriter,
				})
				.SingleOrDefault(song => song.Id == id);
		}

		public IEnumerable<SongInfoDTO> GetSongsByAlbumId(int albumId)
		{
			return _db.Songs
				.Where(song => song.AlbumId == albumId)
				.Select(song => new SongInfoDTO
				{
					Id = song.Id,
					SongName = song.SongName,
					SongCoverPath= song.SongCoverPath,
					SongPath= song.SongPath,
					Status= song.Status,
					AlbumId= albumId,
					IsExplicit= song.IsExplicit,
					Duration= song.Duration,
					Released = song.Released,
					AlbumName = song.Album != null ? song.Album.AlbumName: string.Empty,
				});
		}

		public IEnumerable<SongIndexDTO> GetRecentlyPlayed(int memberId)
		{
			return _db.SongPlayedRecords
				.Where(record => record.MemberId == memberId)
				.OrderByDescending(record => record.PlayedDate)
				.Select(record => record.Song)
				.Select(song => new SongIndexDTO
				{
					Id = song.Id,
					SongName = song.SongName,
					GenreName = song.Genre.GenreName,
					IsExplicit = song.IsExplicit,
					SongCoverPath = song.SongCoverPath,
					SongPath = song.SongPath,
					AlbumId = song.AlbumId ?? 0,
					PlayedTimes = song.SongPlayedRecords.Count(),
					Artistlist = song.SongArtistMetadata.Select(metadata => metadata.Artist).Select(artist => artist.ToInfoVM()),
					Creatorlist = song.SongCreatorMetadata.Select(metadata => metadata.Creator).Select(creator => creator.ToInfoVM()),
				})
				.Take(50)
				.ToList();
		}
	}
}
