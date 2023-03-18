using api.iSMusic.Models.EFModels;
using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.ViewModels.MemberVMs
{
    public class MemberEditVM
    {

        [Required]
        [StringLength(50)]
        public string? MemberNickName { get; set; }

        [StringLength(100)]
        public string? MemberAddress { get; set; }

        [StringLength(10)]
        public string? MemberCellPhone { get; set; }
        
        public DateTime? MemberDateOfBirth { get; set; }

        [Required]
        public int AvatarId { get; set; }

        [Required]
        public bool MemberReceivedMessage { get; set; }

        [Required]
        public bool MemberSharedData { get; set; }

        [Required]
        public bool LibraryPrivacy { get; set; }

        [Required]
        public bool CalenderPrivacy { get; set;}
        
    }
}
