using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Game_Hacking_Engine.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        protected event Action? OnInitialize;
        public void Initialize() => OnInitialize?.Invoke();
    }
}