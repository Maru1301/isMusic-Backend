namespace api.iSMusic.Models.ViewModels.MemberVMs
{
    public class SubscribedPlanVM
    {
        public int SubscriptionPlanId { get; set; }

        public IEnumerable<string>? Emails { get; set; }
    }
}
