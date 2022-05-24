using System.Windows;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    public class GuideInfo
    {
        public FrameworkElement Uc { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string ButtonContent { get; set; } = null!;
        public int? MinWidth { get; set; } = 220;
        public int? MinHeight { get; set; }
    }
}