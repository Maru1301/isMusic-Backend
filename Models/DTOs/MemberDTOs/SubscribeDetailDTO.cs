namespace api.iSMusic.Models.DTOs.MemberDTOs
{
    public class SubscribeDetailDTO
    {
        public int Id{ get; set; }
        public int NumberOfUsers{ get; set; }
        public string? MemberNickName { get; set; }
        public IEnumerable<string>? MemberEmail { get; set; }
        public string PlanName { get; set; } = null!;
        public string Description { get; set;} = null!;
    }
}
