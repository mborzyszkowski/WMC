using System;

namespace WMC.Models
{
    public class WmcToken
    {
        public string Token { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        public static WmcToken CreateToken(string tokenString, string refreshToken, DateTime expirationDate)
        {
            return new WmcToken
            {
                Token = tokenString, 
                RefreshToken = refreshToken, 
                ExpirationDate = expirationDate,
            };
        }

        public WmcToken GeTokenCopy()
        {
            return CreateToken(Token, RefreshToken, ExpirationDate);
        }

        public bool IsTokenExpired =>
            ExpirationDate < DateTime.UtcNow;
    }
}