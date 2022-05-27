using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    [TemplatePart(Name = GuideControlBase.PartBorderBackground, Type = typeof(Border))]
    [TemplatePart(Name = GuideControlBase.PartCanvasHint, Type = typeof(Canvas))]
    public class GuideWindow : Window
    {
        private readonly GuideControlBase _guideControlBase;

        public List<GuideInfo> Guides;

        static GuideWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideWindow),
                new FrameworkPropertyMetadata(typeof(GuideWindow)));
        }

        public GuideWindow(Window targetWindow, List<GuideInfo> guides)
        {
            _guideControlBase = new GuideControlBase(Close, ShowGuide, guides);
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            ShowInTaskbar = false;

            Height = targetWindow.ActualHeight;
            Width = targetWindow.ActualWidth;
            WindowState = targetWindow.WindowState;
            Left = targetWindow.Left;
            Top = targetWindow.Top;
            Owner = targetWindow;
            Guides = guides;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _guideControlBase.BorderBackground = GetTemplateChild(GuideControlBase.PartBorderBackground) as Border;
            _guideControlBase.CanvasHint = GetTemplateChild(GuideControlBase.PartCanvasHint) as Canvas;

            if (Guides.Count <= _guideControlBase.CurrentHintShowIndex)
            {
                return;
            }

            GuideInfo currentGuideInfo = Guides[_guideControlBase.CurrentHintShowIndex];
            if (currentGuideInfo.TargetControl == null)
            {
                return;
            }

            ShowGuide(currentGuideInfo.TargetControl, currentGuideInfo);
        }


        private void ShowGuide(FrameworkElement? targetControl, GuideInfo guide)
        {
            if (targetControl == null)
            {
                return;
            }

            Point point = targetControl.TransformToAncestor(GetWindow(targetControl)!)
                .Transform(new Point(0, 0)); //获取控件坐标点

            RectangleGeometry rg = new() {Rect = new Rect(0, 0, Width, Height)};
            _guideControlBase.CombineHint(rg, targetControl, point);

            GuideHintControl hit = new(this, point, targetControl, guide, Close);
            hit.NextHintEvent -= _guideControlBase.ShowNextHint;
            hit.NextHintEvent += _guideControlBase.ShowNextHint;
            _guideControlBase.CanvasHint?.Children.Add(hit);
        }
    }
}