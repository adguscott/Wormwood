using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wormwood.Models
{
    public class DirectoryModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public int FileCount { get; set; }
        public DirectoryInfo Info { get; set; }

        public DirectoryModel(string path)
        {
            Info = new DirectoryInfo(path);
            Name = Info.Name;
            FullName = path;
            FileCount = Directory.GetFiles(FullName).Length;
        }
    }
}
