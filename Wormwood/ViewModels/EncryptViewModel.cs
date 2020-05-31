using Microsoft.Win32;
using Stylet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Wormwood.Models;

namespace Wormwood.ViewModels
{
    public class EncryptViewModel : Screen
    {
        public ShellViewModel Shell;
        private string Password = "password";
        private FileManager FM;
        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return this._selectedCategory; }
            set { SetAndNotify(ref this._selectedCategory, value); }
        }

        private bool _deleteFileChecked;

        public bool DeleteFileChecked
        {
            get { return this._deleteFileChecked; }
            set { SetAndNotify(ref _deleteFileChecked, value); }
        }

        private FileModel _selectedFile;
        public FileModel SelectedFile
        {
            get { return this._selectedFile; }
            set { SetAndNotify(ref this._selectedFile, value); }
        }
        private string _fileName;
        public string FileName
        {
            get { return this._fileName; }
            set { SetAndNotify(ref this._fileName, value); }
        }

        private BindableCollection<DirectoryModel> _categories;

        public BindableCollection<DirectoryModel> Categories
        {
            get { return this._categories; }
            set { SetAndNotify(ref this._categories, value); }
        }
        private string _iconPath;

        public string IconPath
        {
            get { return this._iconPath; }
            set { SetAndNotify(ref this._iconPath, value); }
        }

        public EncryptViewModel(ShellViewModel shell)
        {
            Shell = shell;
            FM = new FileManager();
            Categories = new BindableCollection<DirectoryModel>(FM.GetCategories());
            IconPath = "pack://application:,,,/Images/file-empty.png";
        }

        public void AddFile()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result == true)
            {
                SelectedFile = new FileModel(dialog.FileName);
                FileName = SelectedFile.Name;
                IconPath = "pack://application:,,,/Images/file-full.png";
            }
        }

        public void EncryptFile()
        {
            if (SelectedFile != null && SelectedCategory != null)
            {
                FM.CreateDirectory(SelectedCategory);
                var category = FM.GetCategory(SelectedCategory);
                var destination = category.FullName + $"\\{SelectedFile.Name}{SelectedFile.Extension}";
                if (!File.Exists(destination))
                {
                    try
                    {
                        FM.Encrypt(Password, SelectedFile, destination);
                        if (DeleteFileChecked)
                        {
                            FM.DeleteFile(SelectedFile.FullName);
                        }
                        Shell.WindowManager.ShowMessageBox($"Successfully Encrypted {SelectedFile.Name}{SelectedFile.Extension}");
                    }
                    catch (Exception)
                    {
                        Shell.WindowManager.ShowMessageBox("Failed to Encrypt File");
                    }
                    finally
                    {
                        Close();
                    }
                }
                else
                {
                    Shell.WindowManager.ShowMessageBox("Encrypted file with this name and category already exists.");
                }
            }
            else
            {
                StringBuilder message = new StringBuilder();
                message.Append("Could not encrypt file: \n");
                if (SelectedFile == null)
                {
                    message.Append("No file added\n");
                }
                if (SelectedCategory == null)
                {
                    message.Append("No category selected.\n");
                }
                Shell.WindowManager.ShowMessageBox(message.ToString());
            }
        }

        public void Close()
        {
            this.RequestClose();
            Shell.ActivateItem(Shell.StartVM);
        }
    }
}
