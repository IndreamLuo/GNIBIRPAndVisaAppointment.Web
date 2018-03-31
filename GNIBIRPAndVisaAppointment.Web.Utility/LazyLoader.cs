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
            object result;

            var hashCode = typeof(Func<T>).GetHashCode();
            if (!LoadedObjects.TryGetValue(hashCode, out result))
            {
                result = getter();
                LoadedObjects[hashCode] = result;
            }

            return (T)result;
        }
    }
}