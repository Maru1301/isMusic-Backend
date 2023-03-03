using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.ViewModels.MemberVMs
{
    public class MemberLoginVM
    {
        [Required]
        [StringLength(50)]
        public string? MemberAccount { get; set; }

        [Required]        
        [StringLength(50)]
        public string? MemberPassword { get; set; }
    }
}
