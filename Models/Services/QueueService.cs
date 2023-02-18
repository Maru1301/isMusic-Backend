using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using static api.iSMusic.Controllers.QueuesController;

namespace api.iSMusic.Models.Services
{
	public class QueueService
	{
		private readonly IQueueRepository _queueRepository;

		private readonly ISongRepository _songRepository;

		private readonly IArtistRepository _artistRepository;
		
		private readonly IAlbumRepository _albumRepository;

		private readonly IPlaylistRepository _playlistRepository;

		public QueueService(IQueueRepository repository,ISongRepository songRepository, IArtistRepository artistRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
            _queueRepository = repository;
			_songRepository = songRepository;
			_artistRepository = artistRepository;
			_albumRepository = albumRepository;
			_playlistRepository = playlistRepository;
		}

		public (bool Success, string Message) AddSongIntoQueue(int queueId, int songId)
		{
			if (!CheckQueueExistence(queueId)) return (false, "佇列不存在");

			if (!CheckSongExistence(songId)) return (false, "歌曲不存在");

            _queueRepository.AddSongIntoQueue(queueId, songId);
			return (true, "新增成功");
		}

		public (bool Success, string Message) AddPlaylistIntoQueue(int queueId, int playlistId)
		{
			if (!CheckQueueExistence(queueId)) return (false, "佇列不存在");

			if (!CheckPlaylistExistence(playlistId)) return (false, "清單不存在");

            _queueRepository.AddPlaylistIntoQueue(queueId, playlistId);
			return (true, "新增成功");
		}

		public (bool Success, string Message) AddAlbumIntoQueue(int queueId, int albumId)
		{
			if (!CheckQueueExistence(queueId)) return (false, "佇列不存在");

			if (!CheckAlbumExistence(albumId)) return (false, "專輯不存在");

            _queueRepository.AddAlbumIntoQueue(queueId, albumId);
			return (true, "新增成功");
		}

		public (bool Success, string Message) ChangeQueueContent(int queueId, int contentId, Condition condition)
		{
			try
			{
				if (CheckQueueExistence(queueId) == false) throw new Exception("佇列不存在");

				if (condition.SingleSong)
				{
					if (CheckSongExistence(contentId) == false) throw new Exception("歌曲不存在");

                    _queueRepository.UpdateQueueBySong(queueId, contentId);
				}
				else if (condition.Artist)
				{
					if(CheckArtistExistence(contentId) == false) throw new Exception("音樂家不存在");

					var popularSongIds = _songRepository
						.GetPopularSongs(contentId, "Artist", 2)
						.Select(song => song.Id)
						.ToList();

					if (popularSongIds.Count == 0) throw new Exception("此表演者尚未有歌曲");

                    _queueRepository.UpdateQueueBySongs(queueId, popularSongIds, "Artist", contentId);
				}
				else if (condition.Album)
				{
					if (CheckAlbumExistence(contentId) == false) throw new Exception("專輯不存在");

					var albumSongIds = _songRepository
						.GetSongsByAlbumId(contentId)
						.Select(song => song.Id)
						.ToList();

					if (albumSongIds.Count == 0) throw new Exception("此專輯內沒有歌曲");

                    _queueRepository.UpdateQueueBySongs(queueId, albumSongIds, "Album", contentId);
				}
				else
				{
					if (CheckPlaylistExistence(contentId) == false) throw new Exception("播放清單不存在");

					var playlistSongIds = _songRepository
						.GetSongsByPlaylistId(contentId)
						.Select(song => song.Id)
						.ToList();

					if (playlistSongIds.Count == 0) throw new Exception("此播放清單內沒有歌曲");

                    _queueRepository.UpdateQueueBySongs(queueId, playlistSongIds, "Playlist", contentId);
				}
			}
			catch(Exception ex)
			{
				return (false, ex.Message);
			}

			return (true, string.Empty);
		}

		public (bool Success, string Message) UpdateByDisplayOredr(int queueId, int displayOrder)
		{
			if (CheckQueueExistence(queueId) == false) return (false, "佇列不存在");

            _queueRepository.UpdateByDisplayOredr(queueId, displayOrder);

			return (true, string.Empty);
		}

        public (bool Success, string Message) NextSong(int queueId)
		{
			try
			{
                if (CheckQueueExistence(queueId) == false) throw new Exception("佇列不存在");

                _queueRepository.NextSong(queueId);
            }
			catch(Exception ex)
			{
				return (false, ex.Message);
			}

			return (true, string.Empty);
        }

        public (bool Success, string Message) PreviousSong(int queueId)
        {
			try
			{
                if (CheckQueueExistence(queueId) == false) return (false, "佇列不存在");

                _queueRepository.PreviousSong(queueId);
            }
            catch(Exception ex)
			{
				return (false, ex.Message);
			}

            return (true, string.Empty);
        }

        public (bool Success, string Message) ChangeShuffle(int queueId)
		{
			if (CheckQueueExistence(queueId) == false) return (false, "佇列不存在");

            _queueRepository.ChangeShuffle(queueId);
			
			return (true, "更新成功");
		}

		public (bool Success, string Message)  ChangeRepeat(int queueId, string mode)
		{
			if (CheckQueueExistence(queueId) == false) return (false, "佇列不存在");

            _queueRepository.ChangeRepeat(queueId, mode);

			return (true, "更新成功");
		}


		private bool CheckQueueExistence(int queueId)
		{
			var queue = _queueRepository.GetQueueByIdForCheck(queueId);

			return queue != null;
		}

		private bool CheckSongExistence(int songId)
		{
			var song = _songRepository.GetSongByIdForCheck(songId);

			return song != null;
		}

		private bool CheckArtistExistence(int contentId)
		{
			var artist = _artistRepository.GetArtistByIdForCheck(contentId);

			return artist != null;
		}

		private bool CheckAlbumExistence(int contentId)
		{
			var album = _albumRepository.GetAlbumByIdForCheck(contentId);

			return album != null;
		}

		private bool CheckPlaylistExistence(int contentId)
		{
			var playlist = _playlistRepository.GetPlaylistByIdForCheck(contentId);

			return playlist != null;
		}

	}
}
