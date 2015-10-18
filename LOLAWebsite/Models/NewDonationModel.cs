using System;
namespace LOLAWebsite.Models
{
    public class NewDonationModel 
    {
        public bool Type { get; set; }
        public string Category { get; set; }
        public string Token { get; set; }
        public  Nullable <float> Amount { get; set; }
        public string CardHolderName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressCity { get; set; }
        public string AddressPostcode { get; set; }
        public string AddressState { get; set; }
    }
}