using System;

namespace WMC.Exceptions
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
