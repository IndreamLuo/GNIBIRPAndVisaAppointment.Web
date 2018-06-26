using System;
using System.Collections.Generic;

namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public abstract class Cache : Dictionary<string, object>, ICache
    {
        public T Cached<T>(Func<T> getter)
        {
            return Cached<T>(typeof(T).FullName, getter);
        }

        public T Cached<T>(string key, Func<T> getter)
        {
            if (!this.TryGetValue(key, out var value))
            {
                value = getter();
                this[key] = value;
            }
            
            return (T)value;
        }
    }

    public class GlobalCache : Cache, IGlobalCache
    {

    }

    public class CurrentObjectCache : Cache, ICurrentObjectCache
    {
        
    }
}