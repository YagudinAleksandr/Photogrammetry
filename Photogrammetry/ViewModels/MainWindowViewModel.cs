﻿using Prism.Mvvm;

namespace Photogrammetry.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Фотограмметрия";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
