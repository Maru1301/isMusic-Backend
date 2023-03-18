using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static api.iSMusic.Controllers.QueuesController;

namespace api.iSMusic.Models.Services
{
	public class QueueService
	{
		private readonly IQueueRepository _queueRepository;

		private readonly ISongRepository _songRepository;

		private readonly IArtistRepository _artistRepository;

		private readonly ICreatorRepository _creatorRepository;
		
		private readonly IAlbumRepository _albumRepository;

		private readonly IPlaylistRepository _playlistRepository;

		private int _memberId;

		public QueueService(IQueueRepository repository,ISongRepository songRepository, IArtistRepository artistRepository, ICreatorRepository creatorRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
            _queueRepository = repository;
			_songRepository = songRepository;
			_artistRepository = artistRepository;
			_creatorRepository = creatorRepository;
			_albumRepository = albumRepository;
			_playlistRepository = playlistRepository;
		}

		public (bool Success, string Message) AddSongIntoQueue(int memberId, int songId)
		{
			if (!CheckQueueExistence(memberId)) return (false, "佇列不存在");

			if (!CheckSongExistence(songId)) return (false, "歌曲不存在");

            _queueRepository.AddSongIntoQueue(memberId, songId);
			return (true, "新增成功");
		}

		public (bool Success, string Message) AddPlaylistIntoQueue(int memberId, int playlistId)
		{
			if (!CheckQueueExistence(memberId)) return (false, "佇列不存在");

			if (!CheckPlaylistExistence(playlistId)) return (false, "清單不存在");

            _queueRepository.AddPlaylistIntoQueue(memberId, playlistId);
			return (true, "新增成功");
		}

		public (bool Success, string Message) AddAlbumIntoQueue(int memberId, int albumId)
		{
			if (!CheckQueueExistence(memberId)) return (false, "佇列不存在");

			if (!CheckAlbumExistence(albumId)) return (false, "專輯不存在");

            _queueRepository.AddAlbumIntoQueue(memberId, albumId);
			return (true, "新增成功");
		}

		public (bool Success, string Message) ChangeQueueContent(int memberId, int contentId, string condition)
		{
			if (CheckQueueExistence(memberId) == false) return (false, "佇列不存在");

			var songIds = new List<int>();
			int takeRow = 2;
			switch (condition)
			{
				case "SingleSong":
                    if (CheckSongExistence(contentId) == false) return (false, "歌曲不存在");

                    _queueRepository.UpdateQueueBySong(memberId, contentId);
					return (true, string.Empty);

                case "Artist":
                case "Creator":
                    songIds = _songRepository
                        .GetPopularSongs(contentId, condition, takeRow)
                        .Select(song => song.Id)
                        .ToList();

                    if (!songIds.Any())return (false, $"此{condition}尚未有歌曲");
                    break;

                case "Album":
                    if (CheckAlbumExistence(contentId) == false) return (false, "專輯不存在");

                    songIds = _songRepository
                        .GetSongsByAlbumId(contentId)
                        .Select(song => song.Id)
                        .ToList();

                    if (!songIds.Any())return (false, "此專輯內沒有歌曲");
                    break;

				case "Playlist":
                    if (CheckPlaylistExistence(contentId) == false) throw new Exception("播放清單不存在");

                    songIds = _songRepository
                        .GetSongsByPlaylistId(contentId)
                        .Select(song => song.Id)
                        .ToList();

                    if (!songIds.Any()) return (false, "此播放清單內沒有歌曲");
					break;

                default:
                    return (false, "不支援的操作");
            }

            _queueRepository.UpdateQueueBySongs(memberId, songIds, condition, contentId);

			return (true, string.Empty);
		}

		public (bool Success, string Message) UpdateByDisplayOredr(int memberId, int displayOrder)
		{
			if (CheckQueueExistence(memberId) == false) return (false, "佇列不存在");

            _queueRepository.UpdateByDisplayOredr(memberId, displayOrder);

			return (true, string.Empty);
		}

        public (bool Success, string Message, SongIndexDTO? Dto) NextSong(int memberId)
		{
			string message = string.Empty;
            int? takeOrder;
            try
            {
                if (CheckQueueExistence(memberId) == false) throw new Exception("佇列不存在");

                int nextSongId;
                (takeOrder, nextSongId) = _queueRepository.NextSong(memberId);

                _songRepository.CreatePlayRecord(memberId, nextSongId);
            }
            catch (Exception ex)
            {
                return (true, ex.Message, null);
            }

            SongIndexDTO? addedQueueSong = null;
            if (takeOrder == null)
            {
                message = "不須增加佇列項目";
			}
			else
			{
                addedQueueSong = _songRepository.GetSongByQueueOrder(memberId, takeOrder.Value);
            }

			return (true, message, addedQueueSong);
        }

        public (bool Success, string Message) PreviousSong(int memberId)
        {
            string message = string.Empty;
            try
			{
                if (CheckQueueExistence(memberId) == false) throw new Exception ("佇列不存在");

                int previoutSongId = _queueRepository.PreviousSong(memberId);

                _songRepository.CreatePlayRecord(memberId, previoutSongId);

            }
            catch(Exception ex)
			{
				return (false, ex.Message);
			}

            return (true, message);
        }

        public (bool Success, string Message) ChangeShuffle(int memberId)
		{
			if (CheckQueueExistence(memberId) == false) return (false, "佇列不存在");

            _queueRepository.ChangeShuffle(memberId);
			
			return (true, "更新成功");
		}

		public (bool Success, string Message)  ChangeRepeat(int queueId)
		{
			if (CheckQueueExistence(queueId) == false) return (false, "佇列不存在");

            _queueRepository.ChangeRepeat(queueId);

			return (true, "更新成功");
		}


		private bool CheckQueueExistence(int memberId)
		{
			var queue = _queueRepository.GetQueueByMemberIdForCheck(memberId);
			_memberId = queue != null ? queue.MemberId : 0;

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

        private bool CheckCreatorExistence(int contentId)
        {
            var artist = _creatorRepository.GetCreatorByIdForCheck(contentId);

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
