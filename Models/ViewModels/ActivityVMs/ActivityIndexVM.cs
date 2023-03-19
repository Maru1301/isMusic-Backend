namespace api.iSMusic.Models.ViewModels.ActivityVMs
{
    public class ActivityIndexVM
    {
        public int Id { get; set; }

        public string ActivityName { get; set; } = null!;

        public DateTime ActivityStartTime { get; set; }

        public DateTime ActivityEndTime { get; set; }

        public string ActivityLocation { get; set; } = null!;

        public int ActivityTypeId { get; set; }

        public string ActivityInfo { get; set; } = null!;

        public int ActivityOrganizerId { get; set; }

        public string ActivityImagePath { get; set; } = null!;

        public DateTime Updated { get; set; }

        public int TotalFollows { get; set; }

        public string ActivityOrganizer { get; set; }

        public string ActivityType { get; set; }
    }
}
