using System;
using System.Collections.Generic;

namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public interface ICache : IDictionary<string, object>
    {
        T Cached<T>(Func<T> getter);

        T Cached<T>(string key, Func<T> getter);
    }

    public interface IGlobalCache : ICache
    {

    }

    public interface ICurrentObjectCache : ICache
    {

    }
}