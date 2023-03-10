using api.iSMusic.Models.DTOs.CartDTOs;
using api.iSMusic.Models.DTOs.ProductDTOs;
using api.iSMusic.Models.EFModels;
using api.iSMusic.Models.Services.Interfaces;
using api.iSMusic.Models.ViewModels.CartVMs;
using api.iSMusic.Models.ViewModels.PlaylistVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.iSMusic.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CartsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("CartItem")]
        public List<CartItemDTO> GetCartInfo()
        {
            int memberId = int.Parse(HttpContext.User.Claims.First(claim=>claim.Type=="MemberId").Value);
            var data = _db.CartItems
                .Include(x=>x.Cart)
                .Include(x=>x.Product)
                .Where(x=>x.Cart.MemberId==memberId)
                .Select(x=> new CartItemDTO
                {
                    Id= x.Id,
                    cartId=x.CartId,
                    ProductName=x.Product.ProductName,
                    ProductPrice=x.Product.ProductPrice,
                    ProductId=x.ProductId,
                    qty = x.Qty
                }).ToList(); 
                

            return data;

        }

        [HttpGet]
        [Route("CartCoupon")]
        public IActionResult GetCouponInfo()
        {

            var data = _db.Coupons.ToList();

            return Ok(data);

        }

        [HttpDelete]
        [Route("DeleteCart/{CartItemtId}")]
        public IActionResult DeleteCarItem(int CartItemtId)
        {
            int memberId = int.Parse(HttpContext.User.Claims.First(claim => claim.Type == "MemberId").Value);
            var data = _db.CartItems.FirstOrDefault(x => x.Cart.MemberId == memberId && x.Id == CartItemtId);

            if (data != null)
            {
                _db.CartItems.Remove(data);
                _db.SaveChanges();
            };

            var newdata = GetCartInfo();
            return Ok(newdata);

        }

        [HttpGet]
        [Route("Checkout")]
        public IActionResult GetCheckoutInfo(int memberId , int couponId)
        {
            if (memberId >0 && couponId > 0)
            {
                var coupondata = _db.Coupons
               .Where(x => x.Id == couponId)
               .First();

                var data = _db.CartItems
                   .Include(x => x.Cart)
                   .Include(x => x.Cart.Member)
                   .Include(x => x.Cart.Member.Orders)
                   .ThenInclude(x => x.Coupon)
                   .Include(x => x.Product)
                   .Where(x => x.Cart.MemberId == memberId)
                   .Select(x => new CheckoutDTO
                   {

                       Id = x.Id,
                       couponText = coupondata.Discounts,
                       ProductName = x.Product.ProductName,
                       ProductPrice = x.Product.ProductPrice,
                       qty = x.Qty,
                       Totalprice = x.Product.ProductPrice * x.Qty

                   }).ToList();


                return Ok(data);
            }
            else
            {

                var data = _db.CartItems
                   .Include(x => x.Cart)
                   .Include(x => x.Cart.Member)
                   .Include(x => x.Cart.Member.Orders)
                   .ThenInclude(x => x.Coupon)
                   .Include(x => x.Product)
                   .Where(x => x.Cart.MemberId == memberId)
                   .Select(x => new CheckoutDTO
                   {

                       Id = x.Id,
                       couponText = null,
                       ProductName = x.Product.ProductName,
                       ProductPrice = x.Product.ProductPrice,
                       qty = x.Qty,
                       Totalprice = x.Product.ProductPrice * x.Qty
                   }).ToList();

                return Ok(data);
            }
            
           
        }

        //[HttpPost]
        //[Route("Cart/{productId}")]
        //public IActionResult AddProductIntoCart(int productId)
        //{
        //    int memberId = int.Parse(HttpContext.User.Claims.First(claim => claim.Type == "MemberId").Value);
        //    try
        //    {
        //        var cartItem = _db.CartItems
        //            .Include(x => x.Cart)
        //            .Include(x => x.Cart.Member)
        //            .Where(x => x.Cart.MemberId == memberId && x.ProductId == productId)
        //            .SingleOrDefault();

        //        if (cartItem != null)
        //        {
        //            //cartItem.Qty++;
        //            cartItem.Qty = cartItem.Qty + numberqty;
        //            _db.SaveChanges();
        //        }
        //        else
        //        {
        //            var newCartItem = new CartItem
        //            {
                       

        //            };
        //            _db.CartItems.Add(newCartItem);
        //            _db.SaveChanges();
        //        }

             

        //        var newcart = GetCartInfo();

        //        return Ok("已加入購物車");
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }


        //}

        [HttpPatch]
        [Route("increaseCart/{productId}")]
        public IActionResult increaseItemQuantity(int productId)
        {
            int memberId = int.Parse(HttpContext.User.Claims.First(claim => claim.Type == "MemberId").Value);
            try
            {
                var cartItem = _db.CartItems
                    .Include(x => x.Cart)
                    .Include(x => x.Cart.Member)
                    .Where(x => x.Cart.MemberId == memberId && x.ProductId == productId)
                    .SingleOrDefault();

                if (cartItem != null)
                {
                    //cartItem.Qty++;
                    cartItem.Qty = cartItem.Qty + 1;
                    _db.SaveChanges();
                }
                else
                {


                    _db.SaveChanges();
                }



                return Ok("已加入購物車");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPatch]
        [Route("decreaseCart/{productId}")]
        public IActionResult decreaseItemQuantity(int productId)
        {
            int memberId = int.Parse(HttpContext.User.Claims.First(claim => claim.Type == "MemberId").Value);
            try
            {
                var cartItem = _db.CartItems
                    .Include(x => x.Cart)
                    .Include(x => x.Cart.Member)
                    .Where(x => x.Cart.MemberId == memberId && x.ProductId == productId)
                    .SingleOrDefault();

                if (cartItem != null)
                {
                    //cartItem.Qty++;
                    cartItem.Qty = cartItem.Qty - 1;
                    _db.SaveChanges();
                }
                else
                {


                    _db.SaveChanges();
                }



                return Ok("購物車數量減1");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

    }
}
