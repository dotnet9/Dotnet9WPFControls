using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    [TemplatePart(Name = GuideControlBase.PartBorderBackground, Type = typeof(Border))]
    [TemplatePart(Name = GuideControlBase.PartCanvasHint, Type = typeof(Canvas))]
    public class GuideControl : Control
    {
        public static readonly DependencyProperty GuidesProperty =
            DependencyProperty.Register(nameof(Guides), typeof(List<GuideInfo>), typeof(GuideControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty DisplayProperty =
            DependencyProperty.Register(nameof(Display), typeof(bool), typeof(GuideControl),
                new PropertyMetadata(false, OnDisplayChanged));

        private readonly GuideControlBase _guideControlBase;

        static GuideControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideControl),
                new FrameworkPropertyMetadata(typeof(GuideControl)));
        }

        public GuideControl()
        {
            _guideControlBase = new GuideControlBase(HideGuide, ShowGuideArea, Guides);
        }

        public List<GuideInfo> Guides
        {
            get => (List<GuideInfo>)GetValue(GuidesProperty);
            set => SetValue(GuidesProperty, value);
        }

        public bool Display
        {
            get => (bool)GetValue(DisplayProperty);
            set => SetValue(DisplayProperty, value);
        }

        private static void OnDisplayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == e.NewValue || d is not GuideControl guideControl)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                guideControl.ShowGuide();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _guideControlBase.BorderBackground = GetTemplateChild(GuideControlBase.PartBorderBackground) as Border;
            _guideControlBase.CanvasHint = GetTemplateChild(GuideControlBase.PartCanvasHint) as Canvas;
        }

        public void HideGuide()
        {
            Visibility = Visibility.Hidden;
            Display = false;
            _guideControlBase.CurrentHintShowIndex = 0;
            _guideControlBase.CanvasHint?.Children.Clear();
        }

        public void ShowGuide()
        {
            _guideControlBase.Guides = Guides;
            HideGuide();
            if (Guides?.Count <= _guideControlBase.CurrentHintShowIndex)
            {
                return;
            }

            GuideInfo currentGuideInfo = Guides![_guideControlBase.CurrentHintShowIndex];
            if (currentGuideInfo.TargetControl == null)
            {
                return;
            }

            Visibility = Visibility.Visible;
            ShowGuideArea(currentGuideInfo.TargetControl, currentGuideInfo);
        }

        private void ShowGuideArea(FrameworkElement? targetControl, GuideInfo guide)
        {
            if (targetControl == null)
            {
                return;
            }

            if (VisualTreeHelper.GetParent(this) is not FrameworkElement container) { return; }

            Point point = targetControl.TransformToAncestor(container).Transform(new Point(0, 0)); //获取控件坐标点

            RectangleGeometry rg = new() {Rect = new Rect(0, 0, container.ActualWidth, container.ActualHeight)};
            _guideControlBase.CombineHint(rg, targetControl, point);

            GuideHintForControl hit = new(this, point, targetControl, guide);
            hit.NextHintEvent -= _guideControlBase.ShowNextHint;
            hit.NextHintEvent += _guideControlBase.ShowNextHint;
            _guideControlBase.CanvasHint?.Children.Add(hit);
        }
    }
}