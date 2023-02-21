using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.ViewModels.MemberVMs
{
    public class MemberRegisterVM
    {        
        [Required]
        [StringLength(50)]
        public string? NickName { get; set; }
        [Required]
        [StringLength(50)]
        public string? Account { get; set; }

        [Required]
        [StringLength(50)]
        //[DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [StringLength(50)]
        [Compare(nameof(Password))]
        //[DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string? Email { get; set; }
    }
}
