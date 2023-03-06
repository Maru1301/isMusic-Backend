namespace api.iSMusic.Models.DTOs.MemberDTOs
{
    public class CreditCardDTO
    {
        public int CreditCardId { get; set; }

        public int? CreditCardNumber { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreditCardHolderName { get; set; }
    }
}
