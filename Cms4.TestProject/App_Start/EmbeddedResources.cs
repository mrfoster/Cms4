using System.Reflection;
using Cms4.EmbeddedResources;
using Cms4.TestProject.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(EmbeddedResources), "PreStart")]

namespace Cms4.TestProject.App_Start
{
    public static class EmbeddedResources
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void PreStart()
        {
            //TODO: set assemblies for embedded resources
            bootstrapper.Assemblies = new[] { Assembly.GetAssembly(typeof (TestPlugin.Models.SignInModel))};
            bootstrapper.Start();
        }
    }
}