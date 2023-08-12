using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Game_Hacking_Engine.ViewModels;
using System;

namespace Game_Hacking_Engine
{
    public class ViewLocator : IDataTemplate
    {
#pragma warning disable CS8767
        public Control Build(object data)
#pragma warning restore CS8767
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

#pragma warning disable IDE0046
            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
#pragma warning restore IDE0046

            return new TextBlock { Text = "Not Found: " + name };
        }

#pragma warning disable CS8767 
        public bool Match(object data)
#pragma warning restore CS8767
        {
            return data is ViewModelBase;
        }
    }
}