using Stylet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Windows.Forms;
using Wormwood.Models;
using Wormwood.Views;

namespace Wormwood.ViewModels
{
    public class LockerViewModel : Stylet.Screen
    {
        public static string Title = "Wormwood: File Locker";
        private ShellViewModel Shell;
        private string Password = "password";
        private ICollectionView FilesView;
        private FileManager FM;
        private BindableCollection<FileModel> _files;
        private BindableCollection<DirectoryModel> _categories;
        private DirectoryModel _selectedCategory;
        public BindableCollection<FileModel> Files
        {
            get { return this._files; }
            set { SetAndNotify(ref this._files, value); }
        }
        public BindableCollection<DirectoryModel> Categories
        {
            get { return this._categories; }
            set { SetAndNotify(ref this._categories, value); }
        }
        public DirectoryModel SelectedCategory
        {
            get { return this._selectedCategory; }
            set { SetAndNotify(ref this._selectedCategory, value); }
        }
        private FileModel _selectedFile;
        public FileModel SelectedFile
        {
            get { return this._selectedFile; }
            set { SetAndNotify(ref this._selectedFile, value); }
        }

        private string _filterString;

        public string FilterString
        {
            get { return this._filterString; }
            set { SetAndNotify(ref this._filterString, value); }
        }

        public LockerViewModel(ShellViewModel shell)
        {
            Shell = shell;
            FM = new FileManager();
            RefreshCategories();
            RefreshFiles();
        }

        public void ChangeCategory()
        {
            if (SelectedCategory != null)
            {
                var category = (SelectedCategory.Name != "All Files") ? FM.GetFiles(SelectedCategory.FullName) : FM.GetAllFiles();
                Files = new BindableCollection<FileModel>(category);
                FilesView = CollectionViewSource.GetDefaultView(Files);
            }
            else
            {
                RefreshFiles();
            }
        }

        public void RefreshFiles()
        {
            Files = new BindableCollection<FileModel>(FM.GetAllFiles());
            FilesView = CollectionViewSource.GetDefaultView(Files);
        }
        public void Delete()
        {
            if (SelectedFile != null)
            {
                DialogResult confirm = MessageBox.Show("Delete", $"Please confirm deletion of file: {SelectedFile.Name}{SelectedFile.Extension}", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    FM.DeleteFile(SelectedFile.FullName);
                    RefreshFiles();
                    RefreshCategories();
                    ChangeCategory();
                }
            }
        }

        public void RefreshCategories()
        {
            Categories = new BindableCollection<DirectoryModel>();
            Categories.Add(FM.GetAllCategory());
            Categories.AddRange(FM.GetCategories());
        }


        public void DeleteDirectory()
        {
            if (SelectedCategory != null && SelectedCategory.Name != "Wormwood")
            {
                if (SelectedCategory.FileCount == 0)
                {
                    FM.DeleteDirectory(SelectedCategory.FullName);
                    Categories.Remove(SelectedCategory);
                    RefreshCategories();
                    ChangeCategory();
                }
                else
                {
                    DialogResult confirm = MessageBox.Show("This category contains files, do you want to delete all files in this category?", "Confirm", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        foreach (FileModel file in FM.GetFiles(SelectedCategory.FullName))
                        {
                            FM.DeleteFile(file.FullName);
                        }
                        SelectedCategory.FileCount = 0;
                        DeleteDirectory();
                    }
                }
            }
        }
        public void Filter()
        {
            if (!string.IsNullOrEmpty(FilterString))
            {
                FilesView.Filter = f =>
                {
                    var file = f as FileModel;
                    return file.Name.StartsWith(FilterString, StringComparison.OrdinalIgnoreCase);
                };
            }
            else
            {
                FilesView.Filter = null;
            }
        }
        public void Close()
        {
            this.RequestClose();
            Shell.ActivateItem(Shell.StartVM);
        }

        public void Decrypt()
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            try
            {
                FM.Decrypt(Password, SelectedFile, dialog.SelectedPath + $"\\{SelectedFile.Name}{SelectedFile.Extension}");
                MessageBox.Show($"Successfully Decrypted {SelectedFile.Name}{SelectedFile.Extension}");
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to Decrypt File");
            }

        }
    }
}
