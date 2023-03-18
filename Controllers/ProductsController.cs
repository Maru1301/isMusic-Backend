using api.iSMusic.Models.DTOs.MusicDTOs;
using api.iSMusic.Models.DTOs.ProductDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Infrastructures.Extensions;
using api.iSMusic.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.iSMusic.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;

        //private readonly List<string> _language = new()
        //{
        //    "中文",
        //    "日語",
        //    "韓文",
        //    "英文"
        //};

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("New")]
        public IEnumerable<ProductIndexDTO> GetProductNewInfo()
        {
            var data = _db.Products
                .Include(product => product.Album)//.ThenInclude(album => album.AlbumType)
                .Include(product => product.Album.AlbumType)
                .Include(product => product.Album.AlbumGenre)
                .Include(product => product.Album.MainArtist)
                .Where(product => product.Status == true)
                .OrderByDescending(product => product.Album.Released)
                .Select(product => new ProductIndexDTO
                {
                    Id = product.Id,
                    CategoryName = product.ProductCategory.CategoryName,
                    ProductPrice = product.ProductPrice,
                    AlbumInfo = product.Album.ToInfoVM(),
                    productName = product.ProductName,
                    stock = product.Stock,
                    status = product.Status,
                }).ToList();
            return data;
        }

        [HttpGet]
        [Route("Popular")]
        public IEnumerable<ProductIndexDTO> GetProductPopularInfo()
        {

            var data = _db.Products
                .Include(product => product.OrderProductMetadata)
                .Include(product => product.Album)
                .Include(product => product.Album.AlbumType)
                .Include(product => product.Album.AlbumGenre)
                .Include(product => product.Album.MainArtist)
                .Where(product => product.Status != false)
                .Select(product => new ProductIndexDTO
                {
                    Id = product.Id,
                    productName = product.ProductName,
                    CategoryName = product.ProductCategory.CategoryName,
                    ProductPrice = product.ProductPrice,
                    stock = product.Stock,
                    AlbumInfo = product.Album.ToInfoVM(),
                    TotalBuyTimes = product.OrderProductMetadata.Count,
                })
                .OrderByDescending(dto => dto.TotalBuyTimes)
                .Take(5)
                .ToList();

            return data;

        }

        [HttpGet]
        [Route("SongGenre/{genreName}")]
        public IEnumerable<ProductIndexDTO> GetSongsGenre(string genreName)
        {

            var data = _db.Products
                .Include(product => product.Album)//.ThenInclude(album => album.AlbumType)
                .Include(product => product.Album.AlbumType)
                .Include(product => product.Album.AlbumGenre)
                .Include(product => product.Album.MainArtist)
                .Where(product => product.Status == true)
                .Where(product => product.Album.AlbumGenre.GenreName == genreName)
                .OrderByDescending(product => product.Album.Released)
                .Select(product => new ProductIndexDTO
                {
                    Id = product.Id,
                    CategoryName = product.ProductCategory.CategoryName,
                    ProductPrice = product.ProductPrice,
                    AlbumInfo = product.Album.ToInfoVM(),
                    productName = product.ProductName,
                    stock = product.Stock,
                    status = product.Status,
                }).ToList();
            return data;
        }

        [HttpGet]
        [Route("ProductSearch/{words}")]
        public IEnumerable<ProductIndexDTO> GetProductSearch([FromRoute] string words , [FromQuery] string Sort)
        {
            var data = _db.Products
                .Include(product => product.Album)//.ThenInclude(album => album.AlbumType)
                    .Include(product => product.Album.AlbumType)
                    .Include(product => product.Album.AlbumGenre)
                    .Include(product => product.Album.MainArtist)
                    .Where(product => product.Status == true);

            data = (Sort == "歌手"
                    ? data.Where(product => product.Album.MainArtist!.ArtistName.Contains(words))
                    : (Sort == "專輯"
                        ? data.Where(product => product.Album.AlbumName.Contains(words))
                        : data));

            var res = data.OrderByDescending(product => product.Album.Released)
                .Select(product => new ProductIndexDTO
                {
                    Id = product.Id,
                    CategoryName = product.ProductCategory.CategoryName,
                    ProductPrice = product.ProductPrice,
                    AlbumInfo = product.Album.ToInfoVM(),
                    productName = product.ProductName,
                    stock = product.Stock,
                    status = product.Status,

                }).ToList();
            return res;

        }


        [HttpGet]
        [Route("{productId}/Detail")]
        public IActionResult GetProductsDetail(int productId)
        {
            var check = _db.Products.Where(x => x.Id == productId).FirstOrDefault();
            if (check == null)
            {
                return NotFound();
            }

            var data = _db.Products
                .Include(product => product.Album)
                .Include(product => product.Album.AlbumType)
                .Include(product => product.Album.AlbumGenre)
                .Include(product => product.Album.MainArtist)
                .Include(product => product.Album.Songs)
                .Where(product => product.Status == true && product.Id == productId)
                .OrderByDescending(product => product.Album.Released)
                .Select(product => new ProductDetailDTO
                {
                    Id = productId,
                    ProductCategoryName = product.ProductCategory.CategoryName,
                    ProductPrice = product.ProductPrice,
                    ProductName = product.ProductName,
                    Stock = product.Stock,
                    AlbumDetail = product.Album.ToDetailVM(),

                }).Single();

            return Ok(data);
        }
    }
}
