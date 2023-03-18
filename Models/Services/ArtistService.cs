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

		private readonly string ArtistMode = "Artist";

		public ArtistService(IArtistRepository artistRepository, ISongRepository songRepository, IAlbumRepository albumRepository, IPlaylistRepository playlistRepository)
		{
			_artistRepository = artistRepository;
			_songRepository = songRepository;
			_albumRepository = albumRepository;
			_playlistRepository = playlistRepository;
		}

        public (bool Success, IEnumerable<ArtistIndexDTO> Dtos) GetRecommended()
		{
			var dtos = _artistRepository.GetRecommended();

			if(!dtos.Any())
			{
				return (false, dtos);
			}

			return (true, dtos);
        }

        public (bool Success, string Message, ArtistDetailDTO dto) GetArtistDetail(int artistId, int memberId)
		{
			var artist = _artistRepository.GetArtistById(artistId);
			if (artist == null) return (false, "表演者不存在", new ArtistDetailDTO());

			var likedArtistIds = _artistRepository.GetLikedArtists(memberId, new Controllers.MembersController.LikedQuery()).Select(lar =>lar.Id);

			if (likedArtistIds.Contains(artistId))
			{
				artist.IsLiked = true;
			}

			var popularSongs = _songRepository.GetPopularSongs(artistId, ArtistMode);

			var likedSongIds = _songRepository.GetLikedSongIdsByMemberId(memberId);

			foreach(var song in popularSongs)
			{
				if (likedSongIds.Contains(song.Id))
				{
					song.IsLiked = true;
				}
			}

			var popularAlbums = _albumRepository.GetPopularAlbums(artistId, ArtistMode);

			var likedAlbumIds = _albumRepository.GetLikedAlbums(memberId, new Controllers.MembersController.LikedQuery()).Select(la => la.Id);

			foreach(var album in popularAlbums)
			{
				if (likedAlbumIds.Contains(album.Id))
				{
					album.IsLiked = true;
				}
			}

			var includedPlaylists = _playlistRepository.GetIncludedPlaylists(artistId, ArtistMode).Where(ip => ip.IsPublic);

			var likedPlaylistIds = _playlistRepository.GetLikedPlaylists(memberId).Select(lp => lp.Id);

			foreach(var playlist in includedPlaylists)
			{
				if (likedPlaylistIds.Contains(playlist.Id))
				{
					playlist.IsLiked = true;
				}
			}

			var dto = new ArtistDetailDTO
			{
				Id= artistId,
				ArtistName = artist.ArtistName,
				ArtistPicPath = artist.ArtistPicPath,
				About = artist.About,
				IsLiked = artist.IsLiked,
				TotalFollowed = artist.TotalFollows,
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

		public (bool Success, string Message, IEnumerable<AlbumIndexDTO> Dtos)GetArtistAlbums(int artistId, int rowNumber)
		{
			if (CheckArtistExistence(artistId) == false) return (false, "表演者不存在", new List<AlbumIndexDTO>());

			if(rowNumber <= 0) return (false, "行數不得為零或是小於零", new List<AlbumIndexDTO>());

			var dtos = _albumRepository.GetAlbumsByContentId(artistId, ArtistMode, rowNumber);
			return (true, string.Empty, dtos);
		}

		public (bool Success, string Message, IEnumerable<PlaylistIndexDTO> Dtos) GetArtistPlaylists(int artistId, int rowNumber)
		{
			if (CheckArtistExistence(artistId) == false) return (false, "表演者不存在", new List<PlaylistIndexDTO>());

			if (rowNumber <= 0) return (false, "行數不得為零或是小於零", new List<PlaylistIndexDTO>());

			var dtos = _playlistRepository.GetIncludedPlaylists(artistId, ArtistMode, rowNumber);
			return (true, string.Empty, dtos);
		}

		public (bool Success, string Message, ArtistAboutDTO Dto) GetArtistAbout(int artistId)
		{
			if (CheckArtistExistence(artistId) == false) return (false, "表演者不存在", new ArtistAboutDTO());

			var dto = _artistRepository.GetArtistAbout(artistId);
			return (true, string.Empty, dto);
		}

		private bool CheckArtistExistence(int artistId)
		{
			var artist = _artistRepository.GetArtistById(artistId);

			return artist != null;
		}
	}
}
