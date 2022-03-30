using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Siemens.Simatic.S7.Webserver.API.Services.FileParser;
using Siemens.Simatic.S7.Webserver.API.Services.WebApp;
using Siemens.Simatic.S7.Webserver.API.Enums;
using Siemens.Simatic.S7.Webserver.API.Services;

namespace WebApiDemo
{
    
    public class UnitTest1 : Base
    {
        [Test]
        public async Task ApiBrowse() 
        {
            var serviceFactory = new ApiStandardServiceFactory();
            var reqHandler = await serviceFactory.GetApiHttpClientRequestHandlerAsync(IPAddress, Username, Password);
            var apiBrowseResponse = await reqHandler.ApiBrowseAsync();
            apiBrowseResponse = reqHandler.ApiBrowse();
            foreach (var method in apiBrowseResponse.Result)
            {
                Console.WriteLine(method.Name);
            }
        }
        
    }
}
