using System.Windows;

namespace Dotnet9WPFControls.Demo.Views
{
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        private void ShowNewGuide_Click(object sender, RoutedEventArgs e)
        {
            UserGuideView window = new UserGuideView
            {
                Title = "无边框窗体测试引导", AllowsTransparency = true, WindowStyle = WindowStyle.None
            };
            window.Show();
        }
    }
}