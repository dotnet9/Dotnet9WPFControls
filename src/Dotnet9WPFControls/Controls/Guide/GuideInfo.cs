using System.Windows;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    public class GuideInfo
    {
        public GuideInfo(string title, string content, string buttonContent = "我知道了", FrameworkElement? element = null,
            int? minWidth = 220,
            int? minHeight = 70)
        {
            Title = title;
            Content = content;
            ButtonContent = buttonContent;
            Uc = element;
            MinWidth = minWidth;
            MinHeight = minHeight;
        }

        public FrameworkElement? Uc { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string ButtonContent { get; set; }
        public int? MinWidth { get; set; }
        public int? MinHeight { get; set; }
    }
}