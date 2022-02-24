using System;

namespace Domain.Exceptions
{
    public class BankTimedOutException : Exception
    {
        public BankTimedOutException(string bankId, string reason)
        {
            Reason = reason;
            BankId = bankId;
        }

        public string Reason { get; }
        public string BankId { get; }
    }
}