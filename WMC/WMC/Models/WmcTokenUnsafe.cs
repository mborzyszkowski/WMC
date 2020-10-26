using System;

namespace WMC.Models
{
    public class WmcTokenUnsafe
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
