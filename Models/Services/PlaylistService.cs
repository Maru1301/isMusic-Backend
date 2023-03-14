using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using Azure.Core;

namespace api.iSMusic.Models.Services
{
    public class PlaylistService
	{
		private readonly IPlaylistRepository _repository;

		private readonly ISongRepository _songRepository;

		private readonly IAlbumRepository _albumRepository;


        private readonly IWebHostEnvironment _webHostEnvironment;

        public PlaylistService(IPlaylistRepository repository, ISongRepository songRepository, IAlbumRepository albumRepository, IWebHostEnvironment webHostEnvironment)
		{
			_repository = repository;
			_songRepository = songRepository;
			_albumRepository = albumRepository;
			_webHostEnvironment = webHostEnvironment;
		}

		public IEnumerable<PlaylistIndexDTO> GetRecommended()
		{
			return _repository.GetRecommended();
		}

		public async Task<int> CreatePlaylistAsync(int memberId)
		{
			var numOfPlaylists = await _repository.GetNumOfPlaylistsByMemberIdAsync(memberId);
			numOfPlaylists += 1;

			var newPlaylist = new PlaylistCreateVM
			{
				MemberId = memberId,
				ListName = "MyPlaylist" + numOfPlaylists
			};

			await _repository.CreatePlaylistAsync(newPlaylist);

			return await _repository.GetPlaylistIdByMemberIdAsync(memberId);
		}

		public (bool Success, string Message, PlaylistDetailDTO Dto) GetPlaylistDetail(int playlistId, int memberId)
		{
			if (playlistId <= 0) return (false, "非法的清單編號", new PlaylistDetailDTO());

			var playlist = _repository.GetPlaylistById(playlistId);
			if (playlist == null)
			{
				return (false, "清單不存在", new PlaylistDetailDTO());
			}
			if(playlist.MemberId == memberId)
			{
				playlist.IsOwner = true;
			}
			else
			{
				playlist.IsOwner = false;
			}

			var likedPlaylists = _repository.GetLikedPlaylists(memberId);
			if(likedPlaylists.Select(playlist => playlist.Id).Contains(playlistId))
			{
				playlist.IsLiked = true;
			}

            var likedSongIds = _songRepository.GetLikedSongIdsByMemberId(memberId);
            foreach (var song in playlist.Metadata.Select(metadata => metadata.Song))
			{
				if (likedSongIds.Contains(song.Id))
				{
					song.IsLiked = true;
				}
			}

			return (true, string.Empty, playlist);
		}

		public IEnumerable<PlaylistIndexDTO> GetPlaylistsByName(string name, int rowNumber)
		{
			int skip = (rowNumber - 1) * 5;
			int take = 5;
			if(rowNumber == 2)
			{
				skip = 0;
				take = 10;
			}

			return _repository.GetPlaylistsByName(name, skip, take);
		}

		public (bool Success, string Message) AddSongToPlaylist(int playlistId, int songId, bool Force)
		{
			var playlist = _repository.GetPlaylistById(playlistId);
			if(playlist == null) return (false, "清單不存在");

			if (CheckSongExistence(songId) == false) return (false, "歌曲不存在");

			var metadata = playlist.Metadata;

			if(Force == false && metadata.Select(metadatum => metadatum.Song.Id).Contains(songId))
			{
				return (false, "歌曲已存在清單內");
			}

			var lastOrder = metadata.Count() != 0 ? metadata.Max(metadatum => metadatum.DisplayOrder) : 0;

			_repository.AddSongToPlaylist(playlistId, songId, lastOrder);

			return (true, "新增成功");
		}

		public (bool Success, string Message) AddAlbumToPlaylist(int playlistId, int albumId, string mode)
		{
			var playlist = _repository.GetPlaylistById(playlistId);
			if (playlist == null) return (false, "播放清單不存在");

			var album = _albumRepository.GetAlbumById(albumId);
			if (album == null) return (false, "專輯不存在");

			var songIdsInPlaylist = playlist.Metadata.Select(metadata => metadata.Song.Id).ToHashSet();

			var selectedSongs = album.Songs.Select(song => song.Id).ToList();

			if (mode == "Normal")
			{
				bool contained = songIdsInPlaylist.IsSupersetOf(selectedSongs);
				if (contained)
				{
					return (false, "整張專輯已經在播放清單中");
				}
				else if (songIdsInPlaylist.Overlaps(selectedSongs))
				{
					return (false, "此播放清單已包部分歌曲");
				}
			}
			else if(mode == "Partial")
			{
				selectedSongs = selectedSongs.Where(songId => songIdsInPlaylist.Contains(songId) == false).ToList();
			}

			var metadata = playlist.Metadata;

			var newOrder = metadata != null ? metadata.Max(metadatum => metadatum.DisplayOrder)+1 : 0;

			_repository.AddSongsToPlaylist(playlistId, selectedSongs, newOrder);
			return (true, "新增成功");
		}

		public (bool Success, string Messgae) UpdatePlaylistDetail(int playlistId, PlaylistEditDTO dto)
		{
			if (CheckPlaylistExistence(playlistId) == false) return (false, "清單不存在");

			if (dto.PlaylistCover != null)
			{
				var parentPath = Directory.GetParent(_webHostEnvironment.ContentRootPath)!.FullName;
				var coverPath = Path.Combine(parentPath, "iSMusic.ServerSide/iSMusic/Uploads/Covers");

                var fileName = Path.GetFileName(dto.PlaylistCover.FileName);
                string newFileName = GetNewFileName(coverPath, fileName);
                var fullPath = Path.Combine(coverPath, newFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
				{
					dto.PlaylistCover.CopyTo(stream);
				}

				dto.PlaylistCoverPath = newFileName;
			}

			_repository.UpdatePlaylistDetail(playlistId, dto);
			return (true, "更新成功");
		}

        private static string GetNewFileName(string path, string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName); // 取得副檔名,例如".jpg"
            string newFileName;
            string fullPath;
            // todo use song name + artists name instead of guid, so when uploading the new file it will replace the old one.
            do
            {
                newFileName = Guid.NewGuid().ToString("N") + ext;
                fullPath = System.IO.Path.Combine(path, newFileName);
            } while (System.IO.File.Exists(fullPath) == true); // 如果同檔名的檔案已存在,就重新再取一個新檔名

            return newFileName;
        }

        public (bool Success, string Message) ChangePrivacySetting(int playlistId)
		{
			if(CheckPlaylistExistence(playlistId)==false) return (false, "清單不存在");

			_repository.ChangePrivacySetting(playlistId);
			return (true, "更新成功");
		}

		public (bool Success, string Message) DeletePlaylist(int playlistId)
		{
			if(CheckPlaylistExistence(playlistId) == false) return(false, "清單不存在");

			_repository.DeletePlaylist(playlistId);
			return (true, "刪除成功");
		}

		public (bool Success, string Message) DeleteSongfromPlaylist(int playlistId, int displayOrder)
		{
			if (CheckPlaylistExistence(playlistId) == false) return (false, "清單不存在");

			_repository.DeleteSongfromPlaylist(playlistId, displayOrder);
			return (true, "刪除成功");
		}

		private bool CheckSongExistence(int songId)
		{
			var song = _songRepository.GetSongByIdForCheck(songId);

			return song != null;
		}

		private bool CheckPlaylistExistence(int playlistId)
		{
			var playlist = _repository.GetPlaylistByIdForCheck(playlistId);
				
			return playlist != null;
		}
	}
}
