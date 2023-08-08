﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using Game_Hacking_Engine.Services;
using System.IO;
using System.Linq;
using FileDialog = Game_Hacking_Engine.Services.FileDialog;

namespace Game_Hacking_Engine.ViewModels
{
    public partial class FileDialogWindowViewModel : ViewModelBase
    {
        string defaultPath = @"C:\";
        public string DefaultPath
        {
            get => defaultPath; set
            {
                defaultPath = value;
                OnPropertyChanged(nameof(DefaultPath));
            }
        }
        public Grid? GridTree { get; set; }
        public TreeView? LeftTreeView { get; set; }
        public TreeView? RightTreeView { get; set; }

        public FileDialogWindowViewModel()
        {
            OnInitialize += () => OpenDirectory(WindowService.ROOT_PATH);
        }

        private void PopulateLeftTreeViewWithDirectories(string path)
        {
            // Free memory!
            LeftTreeView!.Items.Clear();
            // Populate tree view after free treeview(memory)!
            var directories = FileDialog.PopulateTreeViewWithDirectories(path).OrderByDescending(x => x.HasSubDirectories == true);
            if (directories.Any())
            {
                // Show the left column if you have directories.
                ColumnVisibility(true);
                foreach (TreeViewItemPath dir in directories)
                {
                    if (LeftTreeView!.Items.Add(dir.Item) > -1)
                    {
                        // Register the tap event
                        dir.OnDoubleClick += (path) =>
                        {
                            OpenDirectory(path);
                        };
                    }
                }
            }
            else
            {
                // Hide the left column if you don't have directories.
                ColumnVisibility(false);
            }
            DefaultPath = path;
        }

        private void PopulateRightTreeViewWithFiles(string path)
        {
            // Free memory!
            RightTreeView!.Items.Clear();
            // Populate tree view after free treeview(memory)!
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                var item = new TreeViewItemPath(file, Path.GetFileName(file), true);
                RightTreeView!.Items.Add(item.Item);
            }
        }

        private void OpenDirectory(string path)
        {
            try
            {
                if (path.Length >= 3)
                {
                    if (Directory.Exists(path))
                    {
                        PopulateLeftTreeViewWithDirectories(path);
                        PopulateRightTreeViewWithFiles(path);
                    }
                }
                else
                {
                    OpenDirectory(WindowService.ROOT_PATH);
                }
            }
            catch
            {
                OpenDirectory(WindowService.ROOT_PATH);
            }
        }

        public void BackDirectory() // Binding UI
        {
            string? goBack = Path.GetDirectoryName(DefaultPath);
            if (!string.IsNullOrEmpty(goBack))
            {
                OpenDirectory(goBack);
            }
            else
            {
                OpenDirectory(WindowService.ROOT_PATH);
            }
        }

        double clMaxWidth;
        private void ColumnVisibility(bool value)
        {
            ColumnDefinition column = GridTree!.ColumnDefinitions[0];
            if (value == false)
            {
                if (clMaxWidth == 0)
                {
                    clMaxWidth = column.MaxWidth;
                    column.MaxWidth = 0;
                }
            }
            else
            {
                if (clMaxWidth > 0)
                {
                    column.MaxWidth = clMaxWidth;
                    clMaxWidth = 0;
                }
            }
        }

        public void OnTextChanged(object? sender, RoutedEventArgs e)
        {
            OpenDirectory(defaultPath);
        }

        public void OnResized(object? sender, WindowResizedEventArgs e)
        {
            RowDefinition row = GridTree!.RowDefinitions[0];
            double height = e.ClientSize.Height - 40; // 40 -> Top(Header) size!
            row.MinHeight = height;
            row.MaxHeight = height;
        }
    }
}