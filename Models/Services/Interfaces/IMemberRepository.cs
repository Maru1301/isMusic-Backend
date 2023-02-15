using api.iSMusic.Models.DTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.Services.Interfaces
{
	public interface IMemberRepository
	{
		Member? GetMemberById(int memberId);

		Task<Member?> GetMemberAsync(int memberId);

		void AddLikedSong(int memberId, int songId);

		void AddLikedPlaylist(int memberId, int playlistId);

		void AddLikedAlbum(int memberId, int albumId);

		void DeleteLikedSong(int memberId, int songId);

		void DeleteLikedPlaylist(int memberId, int playlistId);

		void DeleteLikedAlbum(int memberId, int albumId);
	}
}
