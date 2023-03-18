using System.ComponentModel.DataAnnotations;

namespace api.iSMusic.Models.ViewModels.MemberVMs
{
    public class MemberResetPasswordVM
    {
        [Required(ErrorMessage = "密碼欄位必填")]
        [StringLength(50)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "確認密碼欄位必填")]
        [StringLength(50)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
