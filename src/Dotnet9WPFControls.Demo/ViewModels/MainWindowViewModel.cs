using Dotnet9WPFControls.Demo.Views;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace Dotnet9WPFControls.Demo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private ICommand? _showGuideControlCommand;
        private ICommand? _showGuideWindowCommand;
        private ICommand? _showRangeObservableCollectionCommand;
        private ICommand? _showShowWrapPanelWithFillCommand;

        public ICommand ShowGuideWindowCommand =>
            _showGuideWindowCommand ??= new DelegateCommand(() => new GuideWindowView().Show());

        public ICommand ShowGuideControlCommand =>
            _showGuideControlCommand ??= new DelegateCommand(() => new GuideControlView().Show());

        public ICommand ShowWrapPanelWithFillCommand =>
            _showShowWrapPanelWithFillCommand ??= new DelegateCommand(() => new WrapPanelWithFillView().Show());

        public ICommand ShowRangeObservableCollectionCommand =>
            _showRangeObservableCollectionCommand ??=
                new DelegateCommand(() => new RangeObservableCollectionView().Show());
    }
}