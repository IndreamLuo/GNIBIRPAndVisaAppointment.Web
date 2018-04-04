using System;
using System.IO;
using GNIBIRPAndVisaAppointment.Web.Business;
using GNIBIRPAndVisaAppointment.Web.Business.Information;
using GNIBIRPAndVisaAppointment.Web.Utility;
using Microsoft.AspNetCore.Rewrite;

namespace GNIBIRPAndVisaAppointment.Web
{
    public class ResourceFileUrlRewriteRule : IRule
    {
        readonly IApplicationSettings ApplicationSettings;
        readonly IDomainHub DomainHub;

        public ResourceFileUrlRewriteRule(IApplicationSettings applicationSettings, IDomainHub domainHub)
        {
            ApplicationSettings = applicationSettings;
            DomainHub = domainHub;
        }

        public void ApplyRule(RewriteContext context)
        {
            var path = context.HttpContext.Request.Path;
            if (path.Value.StartsWith(ApplicationSettings["AppSettings:UploadedFileRewritingURL"]))
            {
                var fileName = Path.GetFileName(path.Value);
                var fileStream = DomainHub.GetDomain<IInformationManager>().LoadFile(fileName);
                var response = context.HttpContext.Response;
                fileStream.CopyTo(response.Body);
            }
        }
    }
}