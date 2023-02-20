using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.Services
{
    public class SongService
	{
		private readonly ISongRepository _songRepository;

		private readonly IMemberRepository _memberRepository;

		public SongService(ISongRepository repo, IMemberRepository memberRepository)
		{
			this._songRepository = repo;
			this._memberRepository = memberRepository;
		}

		public IEnumerable<SongIndexDTO> GetSongsByName(string songName, int rowNumber)
		{
			return _songRepository.GetSongsByName(songName, rowNumber);
		}

		public (bool Success, string ErrorMessage, IEnumerable<SongIndexDTO> RecentlyPlayedSongs) GetRecentlyPlayed(int memberId)
		{
			var member = _memberRepository.GetMemberById(memberId);
			if (member == null)
			{
				return (false, "會員不存在", Enumerable.Empty<SongIndexDTO>());
			}

			var recentlyPlayedSongs = _songRepository.GetRecentlyPlayed(memberId);

			return (true, string.Empty, recentlyPlayedSongs);
		}

		public void CreatePlayRecord(int songId, int memberId)
		{
			_songRepository.CreatePlayRecord(songId, memberId);
		}
    }
}
