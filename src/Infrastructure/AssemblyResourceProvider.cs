using System;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Infrastructure
{
    public class AssemblyResourceProvider : VirtualPathProvider
    {
        protected readonly Assembly _assembly;
        protected readonly Func<string, bool> _isAppResource;

        public AssemblyResourceProvider(Type type) : this(type, path => path.Contains(type.Namespace)) { }

        public AssemblyResourceProvider(Type type, Func<string, bool> isAppResource)
        {
            _assembly = Assembly.GetAssembly(type);
            _isAppResource = isAppResource;
        }

        public override bool FileExists(string virtualPath)
        {
            return (IsAppResourcePath(virtualPath) || base.FileExists(virtualPath));
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            return IsAppResourcePath(virtualPath)
                ? new AssemblyResourceFile(_assembly, virtualPath)
                : base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath,
            IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return IsAppResourcePath(virtualPath)
                ? null
                : base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        protected bool IsAppResourcePath(string virtualPath)
        {
            return !virtualPath.EndsWith(".aspx")
                 && !virtualPath.EndsWith(".ascx")
                 && !virtualPath.EndsWith(".vbhtml")
                 && _isAppResource(VirtualPathUtility.ToAppRelative(virtualPath));
        }
    }
}
