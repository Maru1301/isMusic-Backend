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

		private readonly int skipNumber = 5;

		private readonly int takeNumber = 5;

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

		public IEnumerable<SongIndexDTO> GetPopularSongs(int contentId, string mode, int rowNumber = 1)
		{
			var songs = _db.Songs.Where(s => s.AlbumId != null && s.Status != false);

			if(contentId != 0 && mode == "Artist")
			{
				songs = songs
					.Include(song => song.SongArtistMetadata)
					.Where(song => song.SongArtistMetadata.Select(metadata => metadata.ArtistId).Contains(contentId));
			}else if(contentId != 0 && mode == "Creator")
			{
				songs = songs
					.Include(song => song.SongCreatorMetadata)
					.Where(song => song.SongCreatorMetadata.Select(metadata => metadata.CreatorId).Contains(contentId));
			}

			var dtos = songs
				.Select(s => new SongIndexDTO
				{
					Id = s.Id,
					SongName = s.SongName,
					GenreName = s.Genre.GenreName,
					Duration = s.Duration,
					IsExplicit = s.IsExplicit,
					SongCoverPath = s.SongCoverPath,
					SongPath = s.SongPath,
					AlbumId = s.AlbumId != null ? s.AlbumId.Value : 0,
					PlayedTimes = s.SongPlayedRecords.Count(),
					Artistlist = s.SongArtistMetadata.Select(m => m.Artist.ToInfoVM()).ToList(),
					Creatorlist = s.SongCreatorMetadata.Select(m => m.Creator.ToInfoVM()).ToList(),
				})
				.OrderByDescending(dto => dto.PlayedTimes)
				.Skip(rowNumber == 2 ? 0 :(rowNumber - 1) * skipNumber)
				.Take(rowNumber == 2 ? takeNumber * 2 : takeNumber)
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
			return _db.PlaylistSongMetadata
				.Include(metadata => metadata.Song)
					.ThenInclude(song => song.Album)
				.Where(metadata => metadata.PlayListId == playlistId)
				.OrderBy(metadata=> metadata.DisplayOrder)
				.Select(metadata => metadata.Song)
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

		public IEnumerable<SongIndexDTO> GetSongsByName(string songName, int rowNumber)
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
				.Skip(rowNumber != 2 ? (rowNumber - 1) * skipNumber : 0)
				.Take(rowNumber != 2 ? takeNumber : takeNumber * 2)
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

        public SongIndexDTO? GetSongByQueueOrder(int memberId, int takeOrder)
        {
			var queue = _db.Queues
				.Single(queue => queue.MemberId == memberId);

			var songId = _db.QueueSongs
				.Where(qs => qs.QueueId == queue.Id && ((queue.IsShuffle) ? qs.ShuffleOrder == takeOrder : qs.DisplayOrder == takeOrder))
				.Single().SongId;

			return GetSongById(songId);
		}

        public IEnumerable<SongInfoDTO> GetSongsByAlbumId(int albumId)
		{
			return _db.Songs
				.Where(song => song.AlbumId == albumId)
				.OrderBy(song => song.DisplayOrderInAlbum)
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

        public void CreatePlayRecord(int memberId, int songId)
        {
			var newRecord = new SongPlayedRecord
			{
				SongId = songId,
				MemberId = memberId,
				PlayedDate = DateTime.Now,
			};

			_db.SongPlayedRecords.Add(newRecord);
			_db.SaveChanges();
        }

		public void CreateUploadSong(string coverPath, string songPath, CreatorUploadSongDTO dto)
		{
			var newSong = new Song
			{
				SongName = dto.SongName,
				GenreId= dto.GenreId,
				Duration= dto.Duration,
				IsInstrumental= dto.IsInstrumental,
				Language= dto.Language,
				IsExplicit= dto.IsExplicit,
				Released= dto.Released,
				SongWriter= dto.SongWriter,
				Lyric= dto.Lyric,
				SongCoverPath= coverPath,
				SongPath= songPath,
				Status= dto.Status,
				AlbumId= dto.AlbumId,

			};

			_db.Songs.Add(newSong);
			_db.SaveChanges();
		
		}

	}
}
