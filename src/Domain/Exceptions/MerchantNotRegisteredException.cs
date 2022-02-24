using System;

namespace Domain.Exceptions
{
    public class MerchantNotRegisteredException : Exception
    {
        public string Id { get; }

        public MerchantNotRegisteredException(string id)
        {
            Id = id;
        }
    }
}