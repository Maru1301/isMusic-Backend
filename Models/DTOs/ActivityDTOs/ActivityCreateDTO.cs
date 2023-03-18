namespace api.iSMusic.Models.DTOs.ActivityDTOs
{
    public class ActivityCreateDTO
    {
        public string ActivityName { get; set; } = null!;

        public DateTime ActivityStartTime { get; set; }

        public DateTime ActivityEndTime { get; set; }

        public string ActivityLocation { get; set; } = null!;

        public int ActivityTypeId { get; set; }

        public string ActivityInfo { get; set; } = null!;

        public int ActivityOrganizerId { get; set; }

        public string ActivityImagePath { get; set; } = null!;

        public IFormFile ActivityImage { get; set; } = null!;

        public bool PublishedStatus { get; set; }

        public DateTime Updated { get; set; }
    }
}
