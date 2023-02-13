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

		public MemberService(IMemberRepository repo, IPlaylistRepository playlistRepository, ISongRepository songRepository, IArtistRepository artistRepository)
		{
			_memberRepository = repo;
			_playlistRepository = playlistRepository;
			_songRepository = songRepository;
			_artistRepository = artistRepository;
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

		public (bool Success, string Message, IEnumerable<ArtistIndexDTO> ArtistDtos)GetLikedArtists(int memberId, string condition)
		{
			if(CheckMemberExistence(memberId) == false) return (false, "會員不存在", new List<ArtistIndexDTO>());

			var dtos = _artistRepository.GetLikedArtists(memberId, condition);

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
			try
			{
				var member = _memberRepository.GetMemberById(memberId);

				if (member == null)
				{
					throw new Exception("會員不存在");
				}

				var song = _songRepository.GetSongById(songId);

				if (song == null)
				{
					throw new Exception("歌曲不存在");
				}

				_memberRepository.AddLikedSong(memberId, songId);
			}
			catch(Exception ex)
			{
				return (false, ex.Message);
			}

			return (true, "成功新增");
		}

		public (bool Success, string Message) DeleteLikedSong(int memberId, int songId)
		{
			try
			{
				if (CheckMemberExistence(memberId) == false) throw new Exception("會員不存在");

				if (CheckSongExistence(songId) == false) throw new Exception("歌曲不存在");

				_memberRepository.DeleteLikedSong(memberId, songId);
			}
			catch (Exception ex)
			{
				return (false, ex.Message);
			}

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
	}
}
