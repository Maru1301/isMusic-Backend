using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.ViewModels.MemberVMs
{
    public class MemberInfoVM
    {
        [Required]
        [StringLength(50)]
        public string? NickName { get; set; }
        [Required]
        [StringLength(50)]
        public string? Account { get; set; }

        [Required]
        [StringLength(50)]
        public string? Password { get; set; }

        [Required]
        [StringLength(50)]
        public string? Email { get; set; }
    }
}
