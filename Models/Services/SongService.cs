using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.Infrastructures.Repositories;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.SongVMs;

namespace api.iSMusic.Models.Services
{
	public class SongService
	{
		private readonly ISongRepository _repository;

		public SongService(ISongRepository repo)
		{
			this._repository = repo;
		}
	}
}
