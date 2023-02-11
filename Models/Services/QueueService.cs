using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using static api.iSMusic.Controllers.QueuesController;

namespace api.iSMusic.Models.Services
{
	public class QueueService
	{
		private readonly IQueueRepository _queuerepository;

		private readonly ISongRepository _songRepository;

		private readonly IArtistRepository _artistRepository;
		
		private readonly IAlbumRepository _albumRepository;

		private readonly IPlaylistRepository _playlistRepository;

		public QueueService(IQueueRepository repository,ISongRepository songRepository, IArtistRepository artistRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
			_queuerepository = repository;
			_songRepository = songRepository;
			_artistRepository = artistRepository;
			_albumRepository = albumRepository;
			_playlistRepository = playlistRepository;
		}

		public (bool Success, string Message) ChangeQueueContent(int queueId, int contentId, Condition condition)
		{
			try
			{
				var queue = _queuerepository.GetQueueById(queueId);

				if (queue == null) throw new Exception("佇列不存在");

				if (condition.SingleSong)
				{
					var song = _songRepository.GetSongById(contentId);

					if (song == null) throw new Exception("歌曲不存在");

					_queuerepository.UpdateQueueBySong(queueId, contentId);
				}
				else if (condition.Artist)
				{
					var artist = _artistRepository.GetArtistById(contentId);

					if (artist == null) throw new Exception("音樂家不存在");

					var popularSongIds = _songRepository
						.GetPopularSongs(contentId)
						.Select(song => song.Id)
						.ToList();

					if (popularSongIds.Count == 0) throw new Exception("此表演者尚未有歌曲");

					_queuerepository.UpdateQueueBySongs(queueId, popularSongIds, "Artist", contentId);
				}
				else if (condition.Album)
				{
					var album = _albumRepository.GetAlbumById(contentId);

					if (album == null) throw new Exception("專輯不存在");

					var albumSongIds = _songRepository
						.GetSongsByAlbumId(contentId)
						.Select(song => song.Id)
						.ToList();

					if (albumSongIds.Count == 0) throw new Exception("此專輯內沒有歌曲");

					_queuerepository.UpdateQueueBySongs(queueId, albumSongIds, "Album", contentId);
				}
				else
				{
					var playlist = _playlistRepository.GetPlaylistById(contentId);

					if (playlist == null) throw new Exception("播放清單不存在");

					var playlistSongIds = _songRepository
						.GetSongsByPlaylistId(contentId)
						.Select(song => song.Id)
						.ToList();

					if (playlistSongIds.Count == 0) throw new Exception("此播放清單內沒有歌曲");

					_queuerepository.UpdateQueueBySongs(queueId, playlistSongIds, "Playlist", contentId);
				}
			}
			catch(Exception ex)
			{
				return (false, ex.Message);
			}

			return (true, string.Empty);
		}
	}
}
