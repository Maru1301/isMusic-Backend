using api.iSMusic.Models.EFModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.DTOs.MemberDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }

        //  訂單資料
        public decimal Price { get; set; }

        public string? ProductName { get; set; }

        public int Qty { get; set; }

        public int Stock { get; set; }

        public bool Status { get; set; }

        public int Payments { get; set; }

        public bool OrderStatus { get; set; }

        public bool Paid { get; set; }

        public DateTime Created { get; set; }

        public string? Receiver { get; set; }

        public string? Address { get; set; }

        public string? Cellphone { get; set; }

        //  會員資料
        public int MemberId { get; set; }
       
        public string? MemberNickName { get; set; }

        //  折扣資料
        public string? CouponText { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpiredDate { get; set; }    
        
        public string? Discounts { get; set; }

        //  分類資料
        public string? CategoryName { get; set; }

        

        //public virtual Order Order { get; set; } = null!;

        //public virtual Product Product { get; set; } = null!;
    }
}
