using System;

namespace WebApplication
{
    public class ServiceIdProvider
    {
        public Guid ServiceId { get; }

        public ServiceIdProvider(Guid serviceId)
        {
            ServiceId = serviceId;
        }
    }
}