using System.Windows;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    public class GuideHintForControl : GuideHintBase
    {
        public GuideHintForControl(FrameworkElement ownerContainer, Point point, FrameworkElement targetControl,
            GuideInfo guide) : base(ownerContainer, point, targetControl, guide)
        {
        }

        public override void CloseHint()
        {
            (OwnerContainer as GuideControl)?.HideGuide();
        }
    }
}