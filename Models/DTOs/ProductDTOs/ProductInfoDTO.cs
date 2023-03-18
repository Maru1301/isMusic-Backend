using api.iSMusic.Models.DTOs.ProductDTOs;
using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.DTOs.ProductDTOs
{
    public class ProductInfoDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public int qty { get; set; }
    }
}




