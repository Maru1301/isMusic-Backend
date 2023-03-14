using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models.DTOs.CartDTOs
{
    public class CheckoutDTO
    {
        public int Id { get; set; }

        public string? ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int qty { get; set; }

        public decimal Totalprice { get; set; }

        public string? couponText { get; set; }

    }
}
