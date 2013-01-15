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
        private readonly VirtualPathProvider previous;

        public ResourcePathProvider(IEnumerable<Assembly> assemblies, VirtualPathProvider previous)
        {
            this.previous = previous;
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
            return PathExists(virtualPath) || previous.FileExists(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return PathExists(virtualPath) ? null : previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            return previous.GetDirectory(virtualDir);
        }

        public override bool DirectoryExists(string virtualDir)
        {
            return previous.DirectoryExists(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            return previous.FileExists(virtualPath) ? previous.GetFile(virtualPath) : GetVirtualFile(virtualPath);
        }

        private VirtualFile GetVirtualFile(string virtualPath)
        {
            var key = GetKey(virtualPath);
            return resources.ContainsKey(key) ? new ResourceVirtualFile(virtualPath, resources[key]) : null;
        }

        private bool PathExists(string path)
        {
            return resources.ContainsKey(GetKey(path));
        }

        private static string GetKey(string virtualPath)
        {
            return virtualPath.ToLower().Replace("~", "").Replace('/', '.');
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

            Key = resource.ToLower();

            var prefix = assembly.GetName().Name;
            if (resource.StartsWith(prefix))
            {
                var startIndex = prefix.Length;
                Key = Key.Substring(startIndex, resource.Length - startIndex);
            }
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