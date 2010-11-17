using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;

namespace Infrastructure
{
    public sealed class AssemblyResourceFile : VirtualFile
    {
        private readonly Assembly _assembly;
        private readonly string _path;

        public AssemblyResourceFile(Assembly assembly, string virtualPath)
            : base(virtualPath)
        {
            _assembly = assembly;

            var path = VirtualPathUtility.ToAppRelative(virtualPath);
            var index = path.LastIndexOf('.');

            // Strip the extension, and any preceeding directories
            _path = path.Remove(index, path.Length - index).Split('/').Last();

            // Razor templates are hardcoded for now
            _path += ".cshtml";
        }

        public override Stream Open()
        {
            return _assembly.GetManifestResourceStream(_path);
        }
    }
}
