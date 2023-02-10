using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;

namespace api.iSMusic.Models.Services
{
	public class MemberService
	{
		private readonly IMemberRepository _memberRepository;

		private readonly IPlaylistRepository _playlistRepository;

		public MemberService(IMemberRepository repo, IPlaylistRepository playlistRepository)
		{
			_memberRepository = repo;
			_playlistRepository = playlistRepository;
		}

		public IEnumerable<PlaylistIndexDTO> GetMemberPlaylist(int memberId, bool includeLiked)
		{
			var member = _memberRepository.GetMember(memberId);
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
	}
}
