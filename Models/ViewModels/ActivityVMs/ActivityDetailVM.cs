using api.iSMusic.Models.EFModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.ViewModels.ActivityVMs
{
    public class ActivityDetailVM
    {
        public int Id { get; set; }

        public string ActivityName { get; set; } = null!;

        public DateTime ActivityStartTime { get; set; }

        public DateTime ActivityEndTime { get; set; }

        public string ActivityLocation { get; set; } = null!;

        public int ActivityTypeId { get; set; }

        public string ActivityTypeName { get; set; } = null!;

        public string ActivityInfo { get; set; } = null!;

        public int ActivityOrganizerId { get; set; }

        public string ActivityOrganizerName { get; set; } = null!;

        public string ActivityImagePath { get; set; } = null!;

        public DateTime Updated { get; set; }

        public int TotalFollows { get; set; }
    }
}
