using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    [TemplatePart(Name = PartBorrderBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartCanvasHint, Type = typeof(Canvas))]
    public class GuideControl : Control
    {
        private const string PartBorrderBackground = "PART_Border_Background";
        private const string PartCanvasHint = "PART_Canvas_Hint";

        public static readonly DependencyProperty GuidesProperty =
            DependencyProperty.Register(nameof(Guides), typeof(List<GuideInfo>), typeof(GuideControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty DisplayProperty =
            DependencyProperty.Register(nameof(Display), typeof(bool), typeof(GuideControl),
                new PropertyMetadata(false, OnDisplayChanged));

        private Border? _borderBackground;

        private PathGeometry _borGeometry = new();
        private Canvas? _canvasHint;
        private int _currentHintShowIndex;

        static GuideControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideControl),
                new FrameworkPropertyMetadata(typeof(GuideControl)));
        }

        public List<GuideInfo>? Guides
        {
            get => (List<GuideInfo>?)GetValue(GuidesProperty);
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

            _borderBackground = GetTemplateChild(PartBorrderBackground) as Border;
            _canvasHint = GetTemplateChild(PartCanvasHint) as Canvas;
        }

        public void HideGuide()
        {
            Visibility = Visibility.Hidden;
            Display = false;
            _currentHintShowIndex = 0;
            _canvasHint?.Children.Clear();
        }

        public void ShowGuide()
        {
            Visibility = Visibility.Visible;
            if (Guides?.Count <= _currentHintShowIndex)
            {
                HideGuide();
                return;
            }

            GuideInfo currentGuideInfo = Guides![_currentHintShowIndex];
            if (currentGuideInfo.TargetControl == null)
            {
                HideGuide();
                return;
            }

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

            RectangleGeometry rg =
                new RectangleGeometry { Rect = new Rect(0, 0, container.ActualWidth, container.ActualHeight) };
            _borGeometry = Geometry.Combine(_borGeometry, rg, GeometryCombineMode.Union, null);
            _borderBackground!.Clip = _borGeometry;

            RectangleGeometry rg1 = new RectangleGeometry
            {
                RadiusX = 3,
                RadiusY = 3,
                Rect = new Rect(point.X - 5, point.Y - 5, targetControl.ActualWidth + 7,
                    targetControl.ActualHeight + 7)
            };
            _borGeometry = Geometry.Combine(_borGeometry, rg1, GeometryCombineMode.Exclude, null);

            _borderBackground.Clip = _borGeometry;

            HintForGuideControl hit = new HintForGuideControl(this, point, targetControl, guide);
            hit.NextHintEvent -= Hit_NextHintEvent;
            hit.NextHintEvent += Hit_NextHintEvent;
            _canvasHint?.Children.Add(hit);
        }

        private void Hit_NextHintEvent()
        {
            while (true)
            {
                _canvasHint?.Children.Clear();
                if (_currentHintShowIndex >= Guides?.Count - 1)
                {
                    HideGuide();
                    return;
                }

                _currentHintShowIndex++;

                GuideInfo currentGuideInfo = Guides![_currentHintShowIndex];
                if (currentGuideInfo.TargetControl == null)
                {
                    continue;
                }

                ShowGuideArea(currentGuideInfo.TargetControl, currentGuideInfo);
                break;
            }
        }
    }
}