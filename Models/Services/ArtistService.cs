using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;

namespace api.iSMusic.Models.Services
{
    public class ArtistService
	{
		private readonly IArtistRepository _artistRepository;

		private readonly ISongRepository _songRepository;

		private readonly IAlbumRepository _albumRepository;

		private readonly IPlaylistRepository _playlistRepository;

		public ArtistService(IArtistRepository artistRepository, ISongRepository songRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
			_artistRepository = artistRepository;
			_songRepository = songRepository;
			_albumRepository = albumRepository;
			_playlistRepository = playlistRepository;
		}

		public (bool Success, string Message, ArtistDetailDTO dto) GetArtistDetail(int artistId)
		{
			var artist = _artistRepository.GetArtistById(artistId);
			if (artist == null) return (false, "表演者不存在", new ArtistDetailDTO());

			var popularSongs = _songRepository.GetPopularSongs(artistId);

			var popularAlbums = _albumRepository.GetPopularAlbums(artistId, true);

			var includedPlaylists = _playlistRepository.GetIncludedPlaylists(artistId);

			var dto = new ArtistDetailDTO
			{
				Id= artistId,
				ArtistName = artist.ArtistName,
				ArtistPicPath = artist.ArtistPicPath,
				PopularSongs = popularSongs.ToList(),
				PopularAlbums = popularAlbums.ToList(),
				IncludedPlaylists = includedPlaylists.ToList(),
			};

			return (true, string.Empty, dto);
		}

		public IEnumerable<ArtistIndexDTO> GetArtistsByName(string name, int rowNumber)
		{
			int skip = (rowNumber - 1) * 5;
			int take = 5;
			if(rowNumber == 2)
			{
				skip = 0;
				take = 10;
			}

			return _artistRepository.GetArtistsByName(name, skip, take);
		}

		private bool CheckArtistExistence(int artistId)
		{
			var artist = _artistRepository.GetArtistById(artistId);

			return artist != null;
		}
	}
}
