using api.iSMusic.Models.DTOs.CartDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.AlbumVMs;
using api.iSMusic.Models.ViewModels.CartVMs;

namespace api.iSMusic.Models.Infrastructures.Extensions
{
    public static class CartExts
    {
        public static MemberCartVM ToCartVM(this Cart source)
            => new()
            {
                
            };
    }
}
