using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;

namespace api.iSMusic.Models.Services
{
    public class AlbumService
	{
		private readonly IAlbumRepository _repository;

		private int ItemsInRow = 5;

		private int skip;

		private int take;

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
			this.skip = (rowNumber - 1) * ItemsInRow;
			this.take = 5;
			if(rowNumber == 2)
			{
				this.skip = 0;
				this.take = 10;
			}


			return _repository.GetAlbumsByGenreId(genreId, this.skip, this.take);
		}

		public IEnumerable<AlbumIndexDTO> GetAlbumsByName(string name, int rowNumber)
		{
			this.skip = (rowNumber - 1) * 5;
			this.take = 5;
			if(rowNumber == 2)
			{
				this.skip = 0;
				this.take = 10;
			}

			return _repository.GetAlbumsByName(name, this.skip, this.take);
		}
	}
}
