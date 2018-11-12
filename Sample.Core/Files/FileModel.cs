using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Files
{
    public class FileModel
    {
        public string Filename { get; set; }

        public int ContentLength { get; set; }

        public string ContentType { get; set; }

        public Stream Content { get; set; }

        public FileModel(string filename, string contentType, Stream content)
        {
            Filename = filename;
            ContentLength = (int)content.Length;
            Content = content;
            ContentType = contentType;
        }

        public FileModel(string filename, string contentType, int contentLength, Stream content)
        {
            Filename = filename;
            ContentLength = contentLength;
            Content = content;
            ContentType = contentType;
        }
    }
}
