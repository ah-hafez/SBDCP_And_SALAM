using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace MCS.Framework.Controls
{
    public class EmbeddedVirtualFile : VirtualFile
    {
        private Stream stream;

        public EmbeddedVirtualFile(string virtualPath, Stream stream)
            : base(virtualPath)
        {
            this.stream = stream;
        }

        public override Stream Open()
        {
            return this.stream;
        }
    }
}
