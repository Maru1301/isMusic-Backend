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

            //==================================================================
            //var data = _db.OrderProductMetadata.GroupBy(x => x.ProductId).Select(x => new { key = x.Key, counts = x.Count() }).OrderByDescending(x=>x.counts).Take(2).ToList();

            //int[] data2 =new int[2] ;
            //for(int i = 0; i <=1; i++) {
            //    data2[i] = data[i].key;
            //}

            //// key => productId -> 連結

            //List<ProductIndexDTO> data3 = new List<ProductIndexDTO>();
            //foreach (var item in data2)
            //{
            //    var product = _db.Products
            //    .Include(product => product.Album).ThenInclude(x => x.AlbumType)
            //    .Include(product => product.ProductCategory)
            //    //.Include(product=>product.Album.AlbumType)
            //    .Include(product => product.Album.AlbumGenre)
            //    .Include(product => product.Album.MainArtist)
            //    .FirstOrDefault(x => x.Id == item);
            //    ProductIndexDTO dto = new ProductIndexDTO()
            //    {
            //        Id = product.Id,
            //        CategoryName = product.ProductCategory.CategoryName,
            //        ProductPrice = product.ProductPrice,
            //        Album = product.Album.ToInfoVM(),

            //        productName = product.ProductName,
            //        stock = product.Stock,
            //        status = product.Status,
            //    };
            //    data3.Add(dto);
            //    //.Select(product => new ProductIndexDTO
            //    //{
            //    //    Id = product.Id,
            //    //    CategoryName = product.ProductCategory.CategoryName,
            //    //    ProductPrice = product.ProductPrice,
            //    //    Album = product.Album.ToInfoVM(),

            //    //    productName = product.ProductName,
            //    //    stock = product.Stock,
            //    //    status = product.Status,
            //    //}).ToList();
            //}

            return data;

        }

        //[HttpGet]
        //[Route("ProductCategories")]
        //public IActionResult GetProductCategoriesInfo()
        //{
        //    var productCategories = _db.ProductCategories.ToList();

        //    return Ok(productCategories);
        //}

        //[HttpGet]
        //[Route("SongLanguages")]
        //public IActionResult GetSongslanguages()
        //{
        //    return Ok(_language);
        //}

        //[HttpGet]
        //[Route("Artists/{artistGender}")]
        //public IActionResult GetArtistGenderInfo([FromRoute]bool artistGender)
        //{
        //    var album = _db.Albums
        //        .Include(album => album.MainArtist)
        //        .Where(x=>x.Id == x.MainArtist.Id)
        //        .ToList();

        //    return Ok(album);
        //}
    }
}
