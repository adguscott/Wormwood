using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Wormwood.Models
{
    public class FileManager
    {
        private static string LocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string WWPath = LocalAppData + "\\Wormwood";

        public FileManager()
        {
            CreateDirectory("");
        }

        public void CreateDirectory(string path)
        {
            string fullPath = (path != "") ? WWPath + $"\\{path}" : WWPath;
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        public void DeleteDirectory(string directoryPath)
        {
            Directory.Delete(directoryPath);
        }

        public static void Move()
        {

        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }
        public void Encrypt(string password, FileModel file, string destination)
        {
            var key = Encryptor.CreateKeyFromPassword(password);
            if (!File.Exists(destination))
            {
                Encryptor.Encrypt(file.FullName, destination, key);
            }
            else
            {
                MessageBox.Show("File with same name already encrypted.");
            }

        }

        public void Decrypt(string password, FileModel file, string destination)
        {
            var key = Encryptor.CreateKeyFromPassword(password);
            if (!File.Exists(destination))
            {
                Encryptor.Decrypt(file.FullName, destination, key);
            }
            else
            {
                MessageBox.Show("Decryption failed.");
            }
        }

        public DirectoryModel GetAllCategory()
        {
            var all = new DirectoryModel(WWPath);
            all.FileCount = GetAllFiles().Count();
            all.Name = "All Files";
            return all;
        }
        public DirectoryModel GetCategory(string name)
        {
            return new DirectoryModel(WWPath + $"\\{name}");
        }
        public IEnumerable<DirectoryModel> GetCategories()
        {
            return Directory.GetDirectories(WWPath).Select(dir => new DirectoryModel(dir));
        }

        public IEnumerable<FileModel> GetFiles(string category)
        {
            return Directory.GetFiles(category).Select(file => new FileModel(file));
        }

        public IEnumerable<FileModel> GetAllFiles()
        {
            return Directory.GetDirectories(WWPath).SelectMany(dir => Directory.GetFiles(dir).Select(file => new FileModel(file)));
        }
    }
}
