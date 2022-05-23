using System.Windows;
using System.Windows.Input;

namespace Dotnet9WPFControls.Demo.Views
{
    public partial class UserGuideView : Window
    {
        public UserGuideView()
        {
            InitializeComponent();
        }

        private void DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}