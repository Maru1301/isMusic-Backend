using iSMusic.Models.Infrastructures;

namespace api.iSMusic.Models.DTOs.MemberDTOs
{
    public class MemberRegisterDTO
    {
        public const string SALT = "!@#$$DGTEGYT";

        public int Id { get; set; }

        public string MemberAccount { get; set; } = null!;

        /// <summary>
        /// 密碼,明碼
        /// </summary>
        public string MemberPassword { get; set; } = null!;

        /// <summary>
        /// 加密之後的密碼
        /// </summary>
        public string MemberEncryptedPassword
        {
            get
            {
                string salt = SALT;
                string result = HashUtility.ToSHA256(this.MemberPassword!, salt);
                return result;
            }
        }

        public string? MemberEmail { get; set; }

        public string? MemberNickName { get; set; }

        public string? ConfirmCode { get; set; }
    }
}
