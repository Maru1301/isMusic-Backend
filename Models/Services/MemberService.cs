using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;

namespace api.iSMusic.Models.Services
{
    public class MemberService
	{
		private readonly IMemberRepository _memberRepository;

		private readonly IPlaylistRepository _playlistRepository;

		private readonly ISongRepository _songRepository;

		public MemberService(IMemberRepository repo, IPlaylistRepository playlistRepository, ISongRepository songRepository)
		{
			_memberRepository = repo;
			_playlistRepository = playlistRepository;
			_songRepository = songRepository;
		}

		public IEnumerable<PlaylistIndexDTO> GetMemberPlaylist(int memberId, bool includeLiked)
		{
			var member = _memberRepository.GetMemberById(memberId);
			if (member == null)
			{
				return Enumerable.Empty<PlaylistIndexDTO>();
			}

			var playlists = _playlistRepository.GetMemberPlaylists(memberId);
			if (includeLiked)
			{
				var likedPlaylists = _playlistRepository.GetLikedPlaylists(memberId);
				playlists = playlists.Concat(likedPlaylists);
			}

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

			return (true, "Successfully Added");
		}
	}
}
