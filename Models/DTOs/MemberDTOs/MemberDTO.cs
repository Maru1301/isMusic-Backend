using api.iSMusic.Models.EFModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSMusic.Models.Infrastructures;

namespace api.iSMusic.Models.DTOs.MemberDTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }

        public string MemberNickName { get; set; } = null!;

        public string MemberAccount { get; set; } = null!;

        public string MemberPassword { get; set; } = null!;

        public string? MemberEncryptedPassword { get; set; }

        public string MemberEmail { get; set; } = null!;

        public string? MemberAddress { get; set; }

        public string? MemberCellphone { get; set; }

        public DateTime? MemberDateOfBirth { get; set; }        

        public bool MemberReceivedMessage { get; set; }

        public bool MemberSharedData { get; set; }

        public bool? LibraryPrivacy { get; set; }

        public bool? CalenderPrivacy { get; set; }

        public int? CreditCardId { get; set; }

        public bool IsConfirmed { get; set; }

        public string? ConfirmCode { get; set; }
        
       public int AvatarId { get; set; }

        public IEnumerable<CreditCardDTO>? CreditCard { get; set; }
    }
}
