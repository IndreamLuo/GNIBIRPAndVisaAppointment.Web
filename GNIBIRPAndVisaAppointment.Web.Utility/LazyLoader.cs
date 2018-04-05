using System;
using System.Collections.Generic;

namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public class LazyLoader
    {
        readonly IDictionary<int, object> LoadedObjects;

        public LazyLoader()
        {
            LoadedObjects = new Dictionary<int, object>();
        }

        public T LazyLoad<T>(Func<T> getter)
        {
            return LazyLoad(null, getter);
        }

        public T LazyLoad<T>(string cacheKey, Func<T> getter)
        {
            var hashCode = (cacheKey as object ?? typeof(Func<T>)).GetHashCode();
            if (!LoadedObjects.TryGetValue(hashCode, out var result))
            {
                result = getter();
                LoadedObjects[hashCode] = result;
            }

            return (T)result;
        }
    }
}