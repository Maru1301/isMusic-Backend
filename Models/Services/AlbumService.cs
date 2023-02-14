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

		public IEnumerable<AlbumIndexDTO> GetAlbumsByGenreId(int genreId, int rowNumber)
		{
			return _repository.GetAlbumsByGenreId(genreId, rowNumber);
		}

		public IEnumerable<AlbumIndexDTO> GetAlbumsByName(string name, int rowNumber)
		{
			return _repository.GetAlbumsByName(name, rowNumber);
		}

		public AlbumDetailDTO? GetAlbumById(int albumId)
		{
			return _repository.GetAlbumById(albumId);
		}
	}
}
