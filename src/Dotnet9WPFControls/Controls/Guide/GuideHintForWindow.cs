using System.Windows;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    public class GuideHintForWindow : GuideHintBase
    {
        public GuideHintForWindow(FrameworkElement ownerContainer, Point point, FrameworkElement targetControl,
            GuideInfo guide) : base(ownerContainer, point, targetControl, guide)
        {
        }

        public override void CloseHint()
        {
            Window.GetWindow(this)?.Close();
        }
    }
}