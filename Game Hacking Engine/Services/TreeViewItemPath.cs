using Avalonia.Controls;
using System;

namespace Game_Hacking_Engine.Services
{
    public class TreeViewItemPath
    {
        public event Action<string>? OnDoubleClick;
        public bool HasSubDirectories { get; set; }
        public string ToPath { get; }
        public TreeViewItem Item { get; }

        public TreeViewItemPath(string toPath, string name, bool loop = false)
        {
            Item = new TreeViewItem
            {
                Header = name
            };

            ToPath = toPath;
            if (!loop)
            {
                AddAccessToSubDirectories(this);
            }
        }

        private void AddAccessToSubDirectories(TreeViewItemPath item)
        {
            HasSubDirectories = FileDialog.GetDirectories(item.ToPath).Length > 0;
            if (HasSubDirectories)
            {
                TreeViewItemPath subItem = new(item.ToPath, "...", HasSubDirectories);
                TreeViewItem _subItem_ = subItem.Item;
                if (item.Item.Items.Add(_subItem_) > -1)
                {
                    var tap = () =>
                    {
                        OnDoubleClick?.Invoke(subItem.ToPath);
                    };

                    _subItem_.DoubleTapped += (_, _) => tap();
                }
            }
            else
            {
                var tap = () =>
                {
                    OnDoubleClick?.Invoke(item.ToPath);
                };

                item.Item.DoubleTapped += (_, _) => tap();
            }
        }
    }
}
