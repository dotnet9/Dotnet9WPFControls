using System.Windows;
using System.Windows.Input;

namespace Dotnet9WPFControls.Demo.Views
{
    public partial class GuideControlView : Window
    {
        public GuideControlView()
        {
            InitializeComponent();
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}