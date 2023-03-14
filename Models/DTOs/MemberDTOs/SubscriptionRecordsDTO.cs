namespace api.iSMusic.Models.DTOs.MemberDTOs
{
    public class SubscriptionRecordsDTO
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public int SubscriptionPlanId{ get; set; }

        public DateTime SubscribedTime { get; set; }

        public DateTime SubscribedExpireTime { get; set; }
    }
}
