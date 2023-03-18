using api.iSMusic.Models.DTOs.ProductDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.ViewModels.CartVMs;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.iSMusic.Models.DTOs.CartDTOs
{
    public class CartItemDTO
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = null!;
        public int cartId { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductId { get; set; }

        public int qty { get; set; }

        public string AlbumCoverPath { get; set; }

    }
}
