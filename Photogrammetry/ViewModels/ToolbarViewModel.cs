using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Photogrammetry.ViewModels
{
    public class ToolbarViewModel : BindableBase
    {
        private readonly IRegionManager iregionManager;
        public ToolbarViewModel(IRegionManager regionManager)
        {
            iregionManager = regionManager; 
        }

        private DelegateCommand _firstTaskCommand;
        public DelegateCommand FirstTaskCommand =>
            _firstTaskCommand ?? (_firstTaskCommand = new DelegateCommand(ExecuteFirstTaskCommand));

        void ExecuteFirstTaskCommand()
        {
            iregionManager.RequestNavigate("ContentRegion", "FirstTaskPage");
        }

        private DelegateCommand _secondTaskCommand;
        public DelegateCommand SecondTaskCommand =>
            _secondTaskCommand ?? (_secondTaskCommand = new DelegateCommand(ExecuteSecondTaskCommand));

        void ExecuteSecondTaskCommand()
        {
            iregionManager.RequestNavigate("ContentRegion", "SecondTaskPage");
        }

        private DelegateCommand _thirdTaskCommand;
        public DelegateCommand ThirdTaskCommand =>
            _thirdTaskCommand ?? (_thirdTaskCommand = new DelegateCommand(ExecuteThirdTaskCommand));

        void ExecuteThirdTaskCommand()
        {
            iregionManager.RequestNavigate("ContentRegion", "ThirdTaskPage");
        }
    }
}
