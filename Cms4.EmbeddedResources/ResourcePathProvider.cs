using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Caching;
using System.Web.Hosting;

namespace Cms4.EmbeddedResources
{
    public class ResourcePathProvider : VirtualPathProvider
    {
        private readonly Dictionary<string, EmbeddedFile> resources;

        public ResourcePathProvider(IEnumerable<Assembly> assemblies)
        {
            resources = new Dictionary<string, EmbeddedFile>();
            foreach (var embeddedFile in assemblies.SelectMany(a => a.GetManifestResourceNames().Select(n => new EmbeddedFile(a, n))))
            {
                if (!resources.ContainsKey(embeddedFile.Key))
                {
                    resources.Add(embeddedFile.Key, embeddedFile);
                }
            }
        }

        public override bool FileExists(string virtualPath)
        {
            return resources.ContainsKey(GetKey(virtualPath)) || base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            return base.FileExists(virtualPath) ? base.GetFile(virtualPath) : GetVirtualFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return resources.ContainsKey(GetKey(virtualPath)) ? null :
                base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        private VirtualFile GetVirtualFile(string virtualPath)
        {
            var key = GetKey(virtualPath);
            return resources.ContainsKey(key) ? new ResourceVirtualFile(virtualPath, resources[key]) : null;
        }

        private static string GetKey(string virtualPath)
        {
            return virtualPath.ToLower().TrimStart('~', '/').Replace('/', '.');
        }
    }

    internal class EmbeddedFile
    {
        public string Key { get; private set; }
        public string Resource { get; private set; }
        public Assembly Assembly { get; private set; }

        public EmbeddedFile(Assembly assembly, string resource)
        {
            Assembly = assembly;
            Resource = resource;

            var startIndex = assembly.GetName().Name.Length;
            Key = resource.ToLower().Substring(startIndex, resource.Length - startIndex);
        }

        public Stream Open()
        {
            return Assembly.GetManifestResourceStream(Resource);
        }
    }

    public class ResourceVirtualFile : VirtualFile
    {
        private readonly EmbeddedFile embeddedFile;

        internal ResourceVirtualFile(string virtualPath, EmbeddedFile embeddedFile)
            : base(virtualPath)
        {
            this.embeddedFile = embeddedFile;
        }

        public override Stream Open()
        {
            return embeddedFile.Open();
        }
    }
}