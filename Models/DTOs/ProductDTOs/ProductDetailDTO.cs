using api.iSMusic.Models.ViewModels.AlbumVMs;

namespace api.iSMusic.Models.DTOs.ProductDTOs
{
    public class ProductDetailDTO
    {
        public int Id { get; set; }

        public int ProductCategoryId { get; set; }

        public string? ProductCategoryName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductName { get; set; } = null!;

        public int Stock { get; set; }

        public virtual AlbumDetailVM AlbumDetail { get; set; } = null!;
    }
}
