using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using Game_Hacking_Engine.Services;
using System.IO;
using System.Linq;
using FileDialog = Game_Hacking_Engine.Services.FileDialog;

namespace Game_Hacking_Engine.ViewModels
{
    public partial class FileDialogWindowViewModel : ViewModelBase
    {
        // Manual Bind
        public Grid? GridTree { get; set; } // Binding UI
        public TreeView? LeftTreeView { get; set; } // Binding UI
        public TreeView? RightTreeView { get; set; } // Binding UI

        [ObservableProperty]
        private string? defaultPath = @"C:\";

        public FileDialogWindowViewModel()
        {
            OnInitialize += () => OpenDirectory(WindowService.ROOT_PATH);
        }

        private void PopulateLeftTreeViewWithDirectories(string path)
        {
            if (LeftTreeView != null)
            {
                // Free memory!
                LeftTreeView.Items.Clear();
                // Populate tree view after free treeview(memory)!
                var directories = FileDialog.PopulateTreeViewWithDirectories(path).OrderByDescending(x => x.HasSubDirectories == true);
                if (directories.Any())
                {
                    // Show the left column if you have directories.
                    ColumnVisibility(true);
                    foreach (TreeViewItemPath dir in directories)
                    {
                        if (LeftTreeView.Items.Add(dir.Item) > -1)
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
        }

        private void PopulateRightTreeViewWithFiles(string path)
        {
            if (RightTreeView != null)
            {
                // Free memory!
                RightTreeView.Items.Clear();
                // Populate tree view after free treeview(memory)!
                string[] files = FileDialog.GetFiles(path);
                foreach (string file in files)
                {
                    var item = new TreeViewItemPath(file, Path.GetFileName(file), true);
                    item.Item.DoubleTapped += (_, _) => OpenFile(file);
                    RightTreeView.Items.Add(item.Item);
                }
            }
        }

        private void OpenFile(string path)
        {
            FileDialog.Select(path);
            Window? view = WindowManager.GetWindow(Windows.FileDialog);
            view?.Close();
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
            if (GridTree != null)
            {
                ColumnDefinition column = GridTree.ColumnDefinitions[0];
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
        }

        public void OnTextChanged(object? sender, RoutedEventArgs e)
        {
            OpenDirectory(DefaultPath!);
        }

        public void OnResized(object? sender, WindowResizedEventArgs e)
        {
            if (GridTree != null)
            {
                RowDefinition row = GridTree.RowDefinitions[0];
                double height = e.ClientSize.Height - 40; // 40 -> Top(Header) size!
                row.MinHeight = height;
                row.MaxHeight = height;
            }
        }
    }
}