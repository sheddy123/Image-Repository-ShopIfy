using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageRepo.Models
{
    public class FileImage
    {
        public byte[] Image { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}
