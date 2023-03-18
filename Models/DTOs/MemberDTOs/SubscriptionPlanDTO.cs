namespace api.iSMusic.Models.DTOs.MemberDTOs
{
    public class SubscriptionPlanDTO
    {
        public int Id { get; set; }

        public string PlanName { get; set; } = null;

        public decimal Price { get; set; }

        public int NumberOfUsers { get; set; }

        public string Description{ get; set; } = null;
    }
}
