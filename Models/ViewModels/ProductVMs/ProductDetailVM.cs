using api.iSMusic.Models.EFModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using api.iSMusic.Models.ViewModels.AlbumVMs;

namespace api.iSMusic.Models.ViewModels.ProductVMs
{
    public class ProductDetailVM
    {

        public int Id { get; set; }

        public int ProductCategoryId { get; set; }

        public string? ProductCategoryName { get; set; }

        public decimal ProductPrice { get; set; }

        public string ProductName { get; set; } = null!;

        public int Stock { get; set; }

        public virtual AlbumDetailVM Album { get; set; } = null!;

    }
}
