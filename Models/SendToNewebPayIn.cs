using api.iSMusic.Models.EFModels;

namespace api.iSMusic.Models
{
    public class SendToNewebPayIn
    {
        public string cartId { get; set; }

        public int Total { get; set; }

        public string productName { get; set; }

        public string[] productNames { get; set; }

    }
}
