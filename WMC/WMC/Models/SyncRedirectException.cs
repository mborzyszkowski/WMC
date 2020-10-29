using System;

namespace WMC.Models
{
    public class SyncRedirectException : Exception
    {
        public SyncRedirectException()
        {
        }

        public SyncRedirectException(string message) : base(message)
        {
        }

        public SyncRedirectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
