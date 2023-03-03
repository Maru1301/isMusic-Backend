using api.iSMusic.Models.EFModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.iSMusic.Models.DTOs.MemberDTOs
{
    public class SubscriptionPlanDTO
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        //public int SubscriptionPlanId { get; set; }

        public DateTime SubscribedTime { get; set; }

        //public virtual Member Member { get; set; } = null!;

        //public virtual SubscriptionPlan SubscriptionPlan { get; set; } = null!;

        public string? MemberNickName { get; set; }

        public string? PlanName { get; set; }

        public decimal? Price { get; set; }

        public byte? numberOfUsers { get; set; }

        public string? description { get; set; }
    }
}
