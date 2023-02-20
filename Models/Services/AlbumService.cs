using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;

namespace api.iSMusic.Models.Services
{
    public class AlbumService
	{
		private readonly IAlbumRepository _repository;

		public AlbumService(IAlbumRepository repo)
		{
			_repository= repo;
		}

		public IEnumerable<AlbumIndexDTO> GetRecommended()
		{
			return _repository.GetRecommended();
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

		public AlbumDetailDTO? GetAlbumById(int albumId)
		{
			return _repository.GetAlbumById(albumId);
		}
	}
}
