using api.iSMusic.Models.EFModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.iSMusic.Models.DTOs.MemberDTOs
{
    public class MemberSubscriptionPlanDTO
    {
        public int Id { get; set; }

        public DateTime SubscribedTime { get; set; }

        public DateTime SubscribedExpireTime { get; set; }

        public int MemberId { get; set; }

        public string? MemberNickName { get; set; }

        public string? PlanName { get; set; }

        public decimal? Price { get; set; }

        public int? NumberOfUsers { get; set; }        
        
    }
}
