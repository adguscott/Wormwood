using System;
using System.IO;

namespace Wormwood.Models
{
    public class FileModel
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FullName { get; set; }

        public FileModel(string filePath)
        {
            Name = Path.GetFileNameWithoutExtension(filePath);
            Extension = Path.GetExtension(filePath);
            FullName = filePath;
        }
    }
}
