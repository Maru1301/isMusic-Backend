using api.iSMusic.Models.DTOs;
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

		public IEnumerable<AlbumIndexDTO> GetAlbumsByGenreId(int genreId)
		{
			return _repository.GetAlbumsByGenreId(genreId);
		}
	}
}
