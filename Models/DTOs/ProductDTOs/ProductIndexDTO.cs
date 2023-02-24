using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.AlbumVMs;

namespace api.iSMusic.Models.DTOs.ProductDTOs
{
    public class ProductIndexDTO
    {
        public int Id { get; set; }

        public string? CategoryName { get; set; }

        public decimal ProductPrice { get; set; }

        public AlbumInfoVM? AlbumInfo { get; set; }

        public string? productName { get; set; }

        public int stock { get; set; }

        public bool status { get; set; }

        public int TotalBuyTimes { get; set; }
    }
}
