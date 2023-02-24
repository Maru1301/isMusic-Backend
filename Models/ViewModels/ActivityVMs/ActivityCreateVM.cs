namespace api.iSMusic.Models.ViewModels.ActivityVMs
{
    public class ActivityCreateVM
    {
        public string ActivityName { get; set; } = null!;

        public DateTime ActivityStartTime { get; set; }

        public DateTime ActivityEndTime { get; set; }

        public string ActivityLocation { get; set; } = null!;

        public int ActivityTypeId { get; set; }

        public string ActivityInfo { get; set; } = null!;

        public string ActivityImagePath { get; set; } = null!;

        public IFormFile ActivityImage { get; set; } = null!;
    }
}
