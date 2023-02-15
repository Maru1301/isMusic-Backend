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

		public PlaylistService(IPlaylistRepository repository, ISongRepository songRepository)
		{
			_repository = repository;
			_songRepository = songRepository;
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

		public (bool Success, string Message, PlaylistDetailVM PlaylistDetail) GetPlaylistDetail(int playlistId)
		{
			if (playlistId <= 0) return (false, "非法的清單編號", new PlaylistDetailVM());

			var playlist = _repository.GetPlaylistById(playlistId);
			if (playlist == null)
			{
				return (false, "清單不存在", new PlaylistDetailVM());
			}
			var memberId = playlist.MemberId;
			var likedSongIds = _songRepository.GetLikedSongIdsByMemberId(memberId);
			var songs = _songRepository.GetSongsByPlaylistId(playlistId);
			var playlistDetailVM = playlist.ToDetailVM();
			foreach (var metadata in playlistDetailVM.PlayListSongMetadata)
			{
				var song = songs.Single(s => s.Id == metadata.SongId).ToInfoVM();
				if (likedSongIds.Contains(song.Id))
				{
					song.IsLiked = true;
				}
				metadata.Song = song;
			}
			return (true, string.Empty, playlistDetailVM);
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

		public (bool Success, string Message)AddSongToPlaylist(int playlistId, int songId, bool Force)
		{
			var playlist = _repository.GetPlaylistById(playlistId);
			if(playlist == null) return (false, "清單不存在");

			var metadata = playlist.PlayListSongMetadata;

			if(!Force && metadata.Select(metadatum => metadatum.SongId).Contains(songId))
			{
				return (false, "清單內已有歌曲");
			}

			var lastOrder = metadata != null ? metadata.Max(metadatum => metadatum.DisplayOrder) : 0;

			_repository.AddSongToPlaylist(playlistId, songId, lastOrder);

			return (true, "新增成功");
		}

		public (bool Success, string Messgae) UpdatePlaylistDetail(int playlistId, PlaylistEditDTO dto)
		{
			if (CheckPlaylistExistence(playlistId)) return (false, "清單不存在");

			if (dto.PlaylistCover != null)
			{
				var fileName = Path.GetFileName(dto.PlaylistCover.FileName);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					dto.PlaylistCover.CopyTo(stream);
				}

				dto.PlaylistCoverPath = Path.Combine("uploads", fileName);
			}

			_repository.UpdatePlaylistDetail(playlistId, dto);
			return (true, "更新成功");
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
