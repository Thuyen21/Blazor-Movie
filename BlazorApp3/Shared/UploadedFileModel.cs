using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp3.Shared
{
   public class UploadedFileModel
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
