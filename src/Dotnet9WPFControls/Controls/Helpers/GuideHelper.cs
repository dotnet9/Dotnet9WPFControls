using System.Windows;

namespace Dotnet9WPFControls.Controls.Helpers
{
    public static class GuideHelper
    {
        public static readonly DependencyProperty GuideInfoProperty
            = DependencyProperty.RegisterAttached("GuideInfo",
                typeof(GuideInfo),
                typeof(GuideHelper),
                new UIPropertyMetadata(default(GuideInfo)));

        public static GuideInfo? GetGuideInfo(UIElement dependencyObject)
        {
            return (GuideInfo)dependencyObject.GetValue(GuideInfoProperty);
        }

        public static void SetGuideInfo(UIElement dependencyObject, GuideInfo? guideInfo)
        {
            dependencyObject.SetValue(GuideInfoProperty, guideInfo);
        }
    }
}