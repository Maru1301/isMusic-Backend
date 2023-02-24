using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
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

        private readonly string CreatorMode = "Creator";

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

			var popularSongs = _songRepository.GetPopularSongs(creatorId, CreatorMode);

			var popularAlbums = _albumRepository.GetPopularAlbums(creatorId, CreatorMode);

			var includedPlaylists = _playlistRepository.GetIncludedPlaylists(creatorId, CreatorMode);

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

        public (bool Success, string Message, IEnumerable<AlbumIndexDTO> Dtos) GetCreatorAlbums(int creatorId, int rowNumber)
        {
            if (CheckCreatorExistence(creatorId) == false) return (false, "創作者不存在", new List<AlbumIndexDTO>());

            if (rowNumber <= 0) return (false, "行數不得為零或是小於零", new List<AlbumIndexDTO>());

            var dtos = _albumRepository.GetAlbumsByContentId(creatorId, "Creator", rowNumber);
            return (true, string.Empty, dtos);
        }

        public (bool Success, string Message, IEnumerable<PlaylistIndexDTO> Dtos) GetCreatorPlaylists(int creatorId, int rowNumber)
		{
            if (CheckCreatorExistence(creatorId) == false) return (false, "創作者不存在", new List<PlaylistIndexDTO>());

            if (rowNumber <= 0) return (false, "行數不得為零或是小於零", new List<PlaylistIndexDTO>());

            var dtos = _playlistRepository.GetIncludedPlaylists(creatorId, CreatorMode, rowNumber);
            return (true, string.Empty, dtos);
        }

        private bool CheckCreatorExistence(int creatorId)
        {
            var creator = _creatorRepository.GetCreatorByIdForCheck(creatorId);

			return creator!= null;
        }

		public (bool Success, string Message) CreatorUploadSong(string coverPath, string songPath, CreatorUploadSongDTO creatoruploadsongdto)
		{
			_songRepository.CreateUploadSong(coverPath, songPath, creatoruploadsongdto);

			return (true, "新增歌曲成功");
		}
    }
}
