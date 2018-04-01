using System.Linq;

namespace GNIBIRPAndVisaAppointment.Web.Utility
{
    public static class TableEntityExtension
    {
        public static void ApplyContentFrom<TTableEntity>(this TTableEntity to, TTableEntity from)
        {
            var propertyInfos = typeof(TTableEntity)
                .GetProperties()
                .Where(propertyInfo => propertyInfo.Name != "PartitionKey" && propertyInfo.Name != "RowKey")
                .ToArray();

            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.CanRead && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(to, propertyInfo.GetValue(from));
                }
            }
        }
    }
}