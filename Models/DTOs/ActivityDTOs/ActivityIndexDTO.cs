using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace api.iSMusic.Models.DTOs.ActivityDTOs
{
    public class ActivityIndexDTO
    {
        public int Id { get; set; }

        public string ActivityName { get; set; } = null!;

        public DateTime ActivityStartTime { get; set; }

        public DateTime ActivityEndTime { get; set; }

        public string ActivityLocation { get; set; } = null!;

        public string ActivityType { get; set; } = null!;

        public string ActivityOrganizer { get; set; } = null!;

        public int ActivityTypeId { get; set; }

        public string ActivityInfo { get; set; } = null!;

        public int ActivityOrganizerId { get; set; }

        public string ActivityImagePath { get; set; } = null!;

        public DateTime Updated { get; set; }

        public int TotalFollows { get; set; }
    }
}