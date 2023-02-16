using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using Microsoft.Identity.Client;

namespace api.iSMusic.Models.Services
{
    public class CreatorService
	{
		private readonly ICreatorRepository _creatorRepository;
		private readonly ISongRepository _songRepository;
		private readonly IAlbumRepository _albumRepository;
		private readonly IPlaylistRepository _playlistRepository;

		public CreatorService(ICreatorRepository repository, ISongRepository songRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
			_creatorRepository = repository;
			_songRepository = songRepository;
			_albumRepository = albumRepository;
			_playlistRepository = playlistRepository;
		}

		public (bool Success, string Message, CreatorDetailDTO dto) GetCreatorDetail(int creatorId)
		{
			var creator = _creatorRepository.GetCreatorById(creatorId);
			if (creator == null) return (false, "創作者不存在", new CreatorDetailDTO());

			var creatorMode = "Creator";

			var popularSongs = _songRepository.GetPopularSongs(creatorId, creatorMode);

			var popularAlbums = _albumRepository.GetPopularAlbums(creatorId, creatorMode);

			var includedPlaylists = _playlistRepository.GetIncludedPlaylists(creatorId, creatorMode);

			var dto = new CreatorDetailDTO
			{
				Id = creatorId,
				CreatorName = creator.CreatorName,
				CreatorPicPath = creator.CreatorPicPath,
				PopularSongs = popularSongs.ToList(),
				PopularAlbums = popularAlbums.ToList(),
				IncludedPlaylists = includedPlaylists.ToList(),
			};

			return (true, string.Empty, dto);
		}

		public IEnumerable<CreatorIndexDTO> GetCreatorsByName(string creatorName, int rowNumber)
		{
			return _creatorRepository.GetCreatorsByName(creatorName, rowNumber);
		}
	}
}
