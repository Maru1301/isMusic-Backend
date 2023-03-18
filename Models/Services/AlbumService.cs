using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;

namespace api.iSMusic.Models.Services
{
    public class AlbumService
	{
		private readonly IAlbumRepository _repository;

		private readonly ISongRepository _songRepository;

		public AlbumService(IAlbumRepository repo, ISongRepository songRepository)
		{
			_repository= repo;
			_songRepository= songRepository;
		}

		public IEnumerable<AlbumIndexDTO> GetRecommended()
		{
			var dtos = _repository.GetRecommended();
			
			return dtos;
		}

		public (bool Success, string Message, IEnumerable<AlbumIndexDTO> Dtos)GetAlbumsByGenreId(int genreId, int rowNumber)
		{
            if (rowNumber <= 0)
            {
                return (false, "要求列數不存在", new List<AlbumIndexDTO>());
            }

            var dtos = _repository.GetAlbumsByGenreId(genreId, rowNumber);
			
			
            return (true, string.Empty, dtos);
        }

		public (bool Success, string Message, IEnumerable<AlbumIndexDTO> Dtos) GetAlbumsByName(string name, int rowNumber)
		{
			if(rowNumber <= 0)
			{
				return (false, "要求列數不存在", new List<AlbumIndexDTO>());
			}

			var dtos = _repository.GetAlbumsByName(name, rowNumber);

			return (true, string.Empty, dtos);
		}

		public AlbumDetailDTO? GetAlbumById(int albumId, int memberId)
		{
			var album = _repository.GetAlbumById(albumId);

			if (album == null) return album;

			if(CheckIsLiked(albumId, memberId))
			{
				album.IsLiked = true;
			}

            var likedSongIds = _songRepository.GetLikedSongIdsByMemberId(memberId);
            foreach (var song in album.Songs)
            {
                if (likedSongIds.Contains(song.Id))
                {
                    song.IsLiked = true;
                }
            }

            return album;
		}

		private bool CheckIsLiked(int albumId, int memberId)
		{
			var metadata = _repository.CheckIsLiked(albumId, memberId);

			return metadata != null;
		}
	}
}
