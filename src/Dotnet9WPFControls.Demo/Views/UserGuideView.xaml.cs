using Dotnet9WPFControls.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Dotnet9WPFControls.Demo.Views
{
    public partial class UserGuideView : Window
    {
        public UserGuideView()
        {
            InitializeComponent();

            Loaded += UserGuideView_Loaded;
        }

        private void UserGuideView_Loaded(object sender, RoutedEventArgs e)
        {
            GuideWindow.ShowGuideBox(new List<object> { BtnShowGuide });
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