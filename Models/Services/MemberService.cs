using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using static api.iSMusic.Controllers.MembersController;
using static api.iSMusic.Controllers.QueuesController;

namespace api.iSMusic.Models.Services
{
	public class MemberService
	{
		private readonly IMemberRepository _memberRepository;

		private readonly IPlaylistRepository _playlistRepository;

		private readonly ISongRepository _songRepository;

		private readonly IArtistRepository _artistRepository;

		private readonly ICreatorRepository _creatorRepository;

		private readonly IAlbumRepository _albumRepository;

		public MemberService(IMemberRepository repo, IPlaylistRepository playlistRepository, ISongRepository songRepository, IArtistRepository artistRepository, ICreatorRepository creatorRepository, IAlbumRepository albumRepository)
		{
			_memberRepository = repo;
			_playlistRepository = playlistRepository;
			_songRepository = songRepository;
			_artistRepository = artistRepository;
			_creatorRepository = creatorRepository;
			_albumRepository = albumRepository;
		}

		public IEnumerable<PlaylistIndexDTO> GetMemberPlaylists(int memberId, InputQuery query)
		{
			var member = _memberRepository.GetMemberById(memberId);
			if (member == null)
			{
				return Enumerable.Empty<PlaylistIndexDTO>();
			}

			var playlists = _playlistRepository.GetMemberPlaylists(memberId, query);
			return playlists;
		}

		public (bool Success, string Message, IEnumerable<ArtistIndexDTO> ArtistDtos) GetLikedArtists(int memberId, LikedQuery query)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在", new List<ArtistIndexDTO>());

			var dtos = _artistRepository.GetLikedArtists(memberId, query);

			return (true, string.Empty, dtos);
		}

		public (bool Success, string Message, IEnumerable<CreatorIndexDTO> CreatorsDtos) GetLikedCreators(int memberId, LikedQuery query)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在", new List<CreatorIndexDTO>());

			var dtos = _creatorRepository.GetLikedCreators(memberId, query);

			return (true, "", dtos);
		}

		public (bool Success, string Message, IEnumerable<AlbumIndexDTO> AlbumsDtos) GetLikedAlbums(int memberId, LikedQuery query)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在", new List<AlbumIndexDTO>());

			var dtos = _albumRepository.GetLikedAlbums(memberId, query);

			return (true, "", dtos);
		}

		public IEnumerable<PlaylistIndexDTO> GetMemberPlaylistsByName(int memberId, string name, int rowNumber)
		{
			var member = _memberRepository.GetMemberById(memberId);
			if (member == null)
			{
				return Enumerable.Empty<PlaylistIndexDTO>();
			}

			var playlists = _playlistRepository.GetMemberPlaylistsByName(memberId, name, rowNumber);

			return playlists;
		}

		public (bool Success, string Message) AddLikedSong(int memberId, int songId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckSongExistence(songId) == false) return (false, "歌曲不存在");

			_memberRepository.AddLikedSong(memberId, songId);
			return (true, "成功新增");
		}

		public (bool Success, string Message) AddLikedPlaylist(int memberId, int playlistId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckPlaylistExistence(playlistId) == false) return (false, "播放清單不存在");

			_memberRepository.AddLikedPlaylist(memberId, playlistId);
			return (true, "成功新增");
		}

		public (bool Success, string Message) AddLikedAlbum(int memberId, int albumId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckAlbumExistence(albumId) == false) return (false, "專輯不存在");

			_memberRepository.AddLikedAlbum(memberId, albumId);
			return (true, "成功新增");
		}

		public (bool Success, string Message) FollowArtist(int memberId, int artistId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckArtistExistence(artistId) == false) return (false, "表演者不存在");

			_memberRepository.FollowArtist(memberId, artistId);
			return (true, "成功新增");
		}

        public (bool Success, string Message) FollowCreator(int memberId, int creatorId)
        {
            if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

            if (CheckCreatorExistence(creatorId) == false) return (false, "表演者不存在");

            _memberRepository.FollowCreator(memberId, creatorId);
            return (true, "成功新增");
        }

        public (bool Success, string Message) DeleteLikedSong(int memberId, int songId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckSongExistence(songId) == false) return (false, "歌曲不存在");

			_memberRepository.DeleteLikedSong(memberId, songId);
			return (true, "成功刪除");
		}

		public (bool Success, string Message) DeleteLikedPlaylist(int memberId, int playlistId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckPlaylistExistence(playlistId) == false) return (false, "播放清單不存在");

			_memberRepository.DeleteLikedPlaylist(memberId, playlistId);
			return (true, "成功刪除");
		}

		public (bool Success, string Message) DeleteLikedAlbum(int memberId, int albumId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckAlbumExistence(albumId) == false) return (false, "專輯不存在");

			_memberRepository.DeleteLikedAlbum(memberId, albumId);
			return (true, "成功刪除");
		}

		public (bool Success, string Message) UnfollowArtist(int memberId, int artistId)
		{
			if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

			if (CheckArtistExistence(artistId) == false) return (false, "表演者不存在");

			_memberRepository.UnfollowArtist(memberId, artistId);
			return (true, "成功刪除");
		}

        public (bool Success, string Message) UnfollowCreator(int memberId, int creatorId)
        {
            if (CheckMemberExistence(memberId) == false) return (false, "會員不存在");

            if (CheckCreatorExistence(creatorId) == false) return (false, "表演者不存在");

            _memberRepository.UnfollowCreator(memberId, creatorId);
            return (true, "成功刪除");
        }

        private bool CheckSongExistence(int songId)
		{
			var song = _songRepository.GetSongById(songId);

			return song != null;
		}

		private bool CheckMemberExistence(int memberId)
		{
			var member = _memberRepository.GetMemberById(memberId);

			return member != null;
		}

		private bool CheckPlaylistExistence(int playlistId)
		{
			var playlist = _playlistRepository.GetPlaylistByIdForCheck(playlistId);

			return playlist != null;
		}

		private bool CheckAlbumExistence(int albumId)
		{
			var playlist = _albumRepository.GetAlbumByIdForCheck(albumId);

			return playlist != null;
		}

		private bool CheckArtistExistence(int artistId)
		{
			var artist = _artistRepository.GetArtistByIdForCheck(artistId);

			return artist != null;
		}

		private bool CheckCreatorExistence(int creatorId)
		{
			var creator = _creatorRepository.GetCreatorByIdForCheck(creatorId);

			return creator != null;
		}
	}
}
