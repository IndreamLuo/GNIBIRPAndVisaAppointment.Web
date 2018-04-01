using GNIBIRPAndVisaAppointment.Web.Utility;
using StructureMap;

namespace GNIBIRPAndVisaAppointment.Web
{
    internal class StructureMapDIContainer : IDIContainer
    {
        readonly IContainer StructureMapContainer;

        internal StructureMapDIContainer(IContainer structureMapContainer)
        {
            StructureMapContainer = structureMapContainer;
        }

        public T GetInstance<T>()
        {
            return StructureMapContainer.GetInstance<T>();
        }
    }
}