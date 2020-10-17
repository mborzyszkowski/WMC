using System;

namespace WMC.Models
{
    public class WmcToken
    {
        public string Token { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        public static WmcToken CreateToken(string tokenString, DateTime expirationDate)
        {
            return new WmcToken
            {
                Token = tokenString, 
                ExpirationDate = expirationDate
            };
        }

        public WmcToken GeTokenCopy()
        {
            return CreateToken(Token, ExpirationDate);
        }

        public bool IsTokenExpired =>
            ExpirationDate < DateTime.Now;
    }
}