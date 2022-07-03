using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Siemens.Simatic.S7.Webserver.API.Services.FileParser;
using Siemens.Simatic.S7.Webserver.API.Services.WebApp;
using Siemens.Simatic.S7.Webserver.API.Enums;
using Siemens.Simatic.S7.Webserver.API.Services;
using System.IO;

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

        [Test]
        public async Task Api_WebAppDeployer_KeepUpdatingTheApp()
        {
            var serviceFactory = new ApiStandardServiceFactory();
            var requestHandler = serviceFactory.GetApiHttpClientRequestHandler(IPAddress, Username, Password);

            var appDir = Path.Combine(CurrentExeDir.Parent.Parent.FullName, "_WebApps", "customerExample");
            var configParser = new ApiWebAppConfigParser(appDir, "WebAppConfig.json", new ApiWebAppResourceBuilder());
            var app = configParser.Parse();

            var deployer = serviceFactory.GetApiWebAppDeployer(requestHandler);
            //for a full deploy: requestHandler.WebAppDelete(app);
            while (true)
            {
                app = configParser.Parse();
                await deployer.DeployOrUpdateAsync(app);
                Thread.Sleep(100);
                var apps = requestHandler.WebAppBrowse().Result.Applications;
                var appResources = requestHandler.WebAppBrowseResources(app).Result.Resources;
            }
        }

        public static DirectoryInfo CurrentExeDir
        {
            get
            {
                string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                return (new FileInfo(dllPath)).Directory;
            }
        }
    }
}
