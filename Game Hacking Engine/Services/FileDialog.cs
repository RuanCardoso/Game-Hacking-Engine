using System.Collections.Generic;
using System.IO;

namespace Game_Hacking_Engine.Services
{
    internal class FileDialog
    {
        static readonly EnumerationOptions enumerationOptions = new()
        {
            IgnoreInaccessible = true,
        };

        public static string[] GetDirectories(string path, string pattern = "*")
        {
            return Directory.GetDirectories(path, pattern, enumerationOptions);
        }

        public static string[] GetFiles(string path, string pattern = "*")
        {
            return Directory.GetFiles(path, pattern, enumerationOptions);
        }

        public static IEnumerable<TreeViewItemPath> PopulateTreeViewWithDirectories(string path)
        {
            string[] directories = GetDirectories(path);
            for (int i = 0; i < directories.Length; i++)
            {
                var dir = directories[i];
                TreeViewItemPath treeViewItemPath = new(dir, Path.GetFileName(dir));
                yield return treeViewItemPath;
            }
        }
    }
}
