using api.iSMusic.Models.DTOs.MusicDTOs;
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
			int skip = (rowNumber- 1) * 5;
			int take = 5;
			if(rowNumber == 2)
			{
				skip = 0;
				take = 10;
			}

			return _songRepository.GetSongsByName(songName, skip, take);
		}

		public (bool Success, string ErrorMessage, IEnumerable<SongIndexDTO> RecentlyPlayedSongs) GetRecentlyPlayed(int memberId)
		{
			var member = _memberRepository.GetMember(memberId);
			if (member == null)
			{
				return (false, "會員不存在", Enumerable.Empty<SongIndexDTO>());
			}

			var recentlyPlayedSongs = _songRepository.GetRecentlyPlayed(memberId);

			return (true, string.Empty, recentlyPlayedSongs);
		}
	}
}
