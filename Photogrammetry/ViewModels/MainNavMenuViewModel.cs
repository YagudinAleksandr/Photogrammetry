using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Photogrammetry.ViewModels
{
    public class MainNavMenuViewModel : BindableBase
    {
        public Action Close { get; set; }
        public MainNavMenuViewModel()
        {

        }

        private DelegateCommand _closeAppCommand;
        public DelegateCommand CloseAppCommand =>
            _closeAppCommand ?? (_closeAppCommand = new DelegateCommand(ExecuteCloseAppCommand));

        void ExecuteCloseAppCommand()
        {

        }
    }
}
