using System.Reflection;
using System.Web.Hosting;
using Cms4.EmbeddedResources;
using Cms4.TestProject.App_Start;

[assembly: WebActivator.PostApplicationStartMethod(typeof(EmbeddedResources), "PostStart")]

namespace Cms4.TestProject.App_Start
{
    public static class EmbeddedResources
    {
        public static void PostStart()
        {
            HostingEnvironment.RegisterVirtualPathProvider(
                new ResourcePathProvider(new[]
                    {
                        Assembly.GetAssembly(typeof (TestPlugin.Models.SignInModel))
                    }));
        }
    }
}