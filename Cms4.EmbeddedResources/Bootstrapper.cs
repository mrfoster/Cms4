using System.Collections.Generic;
using System.Reflection;
using System.Web.Hosting;

namespace Cms4.EmbeddedResources
{
    public class Bootstrapper
    {
        public void Start()
        {
            HostingEnvironment.RegisterVirtualPathProvider(
                new ResourcePathProvider(Assemblies, HostingEnvironment.VirtualPathProvider));
        }

        public IEnumerable<Assembly> Assemblies { get; set; }
    }
}
