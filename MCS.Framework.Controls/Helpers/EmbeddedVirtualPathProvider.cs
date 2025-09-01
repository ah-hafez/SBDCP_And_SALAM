using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Hosting;

namespace MCS.Framework.Controls
{
    public class EmbeddedVirtualPathProvider : VirtualPathProvider
    {
        private VirtualPathProvider previous;

        public EmbeddedVirtualPathProvider(VirtualPathProvider previous)
        {
            this.previous = previous;
        }

        public override bool FileExists(string virtualPath)
        {
            if (IsEmbeddedPath(virtualPath))
                return true;

            return this.previous.FileExists(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsEmbeddedPath(virtualPath))
            {
                return null;
            }

            return this.previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            return this.previous.GetDirectory(virtualDir);
        }

        public override bool DirectoryExists(string virtualDir)
        {
            return this.previous.DirectoryExists(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsEmbeddedPath(virtualPath))
            {
                string nameSpace = typeof(EmbeddedVirtualPathProvider).Namespace;
                string moduleName = virtualPath.Substring(virtualPath.IndexOf('?') + 1);
                string fileNameWithExtension = virtualPath.Substring(virtualPath.LastIndexOf("/") + 1);
                int length = fileNameWithExtension.Length - moduleName.Length;
                string fileExtension = virtualPath.Substring(virtualPath.LastIndexOf("/") + 1, length - 1);
                string manifestResourceName = string.Format("{0}.{1}.{2}", nameSpace, moduleName, fileExtension);
                var stream = typeof(EmbeddedVirtualPathProvider).Assembly.GetManifestResourceStream(manifestResourceName);

                return new EmbeddedVirtualFile(virtualPath, stream);
            }

            return this.previous.GetFile(virtualPath);
        }

        private bool IsEmbeddedPath(string path)
        {
            return path.Contains("~/MCS.Framework.Controls");
        }
    }
}
