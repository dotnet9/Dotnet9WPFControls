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
        private ICommand? _showShowWrapPanelWithFillCommand;

        public ICommand ShowGuideWindowCommand =>
            _showGuideWindowCommand ??= new DelegateCommand(ExecuteShowGuideWindowCommand);

        public ICommand ShowGuideControlCommand =>
            _showGuideControlCommand ??= new DelegateCommand(ExecuteShowGuideControlCommand);

        public ICommand ShowWrapPanelWithFillCommand =>
            _showShowWrapPanelWithFillCommand ??= new DelegateCommand(ExecuteShowWrapPanelWithFillCommand);


        private void ExecuteShowGuideWindowCommand()
        {
            new GuideWindowView().Show();
        }

        private void ExecuteShowGuideControlCommand()
        {
            new GuideControlView().Show();
        }

        private void ExecuteShowWrapPanelWithFillCommand()
        {
            new WrapPanelWithFillView().Show();
        }
    }
}