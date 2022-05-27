using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    [TemplatePart(Name = PartBorrderBackground, Type = typeof(Border))]
    [TemplatePart(Name = PartCanvasHint, Type = typeof(Canvas))]
    public class GuideWindow : Window
    {
        private const string PartBorrderBackground = "PART_Border_Background";
        private const string PartCanvasHint = "PART_Canvas_Hint";
        private readonly List<GuideInfo> _guideInfos;

        private Border? _borderBackground;

        private PathGeometry _borGeometry = new();
        private Canvas? _canvasHint;
        private int _currentHintShowIndex;

        static GuideWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideWindow),
                new FrameworkPropertyMetadata(typeof(GuideWindow)));
        }

        public GuideWindow(Window targetWindow, List<GuideInfo> guideList)
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            ShowInTaskbar = false;

            Height = targetWindow.ActualHeight;
            Width = targetWindow.ActualWidth;
            WindowState = targetWindow.WindowState;
            Left = targetWindow.Left;
            Top = targetWindow.Top;
            Owner = targetWindow;
            _guideInfos = guideList;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _borderBackground = GetTemplateChild(PartBorrderBackground) as Border;
            _canvasHint = GetTemplateChild(PartCanvasHint) as Canvas;

            if (_guideInfos.Count <= _currentHintShowIndex)
            {
                return;
            }

            GuideInfo currentGuideInfo = _guideInfos[_currentHintShowIndex];
            if (currentGuideInfo.TargetControl == null)
            {
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

            Point point = targetControl.TransformToAncestor(GetWindow(targetControl)!)
                .Transform(new Point(0, 0)); //获取控件坐标点

            RectangleGeometry rg = new() {Rect = new Rect(0, 0, Width, Height)};
            _borGeometry = Geometry.Combine(_borGeometry, rg, GeometryCombineMode.Union, null);
            _borderBackground!.Clip = _borGeometry;

            RectangleGeometry rg1 = new()
            {
                RadiusX = 3,
                RadiusY = 3,
                Rect = new Rect(point.X, point.Y, targetControl.ActualWidth,
                    targetControl.ActualHeight)
            };
            _borGeometry = Geometry.Combine(_borGeometry, rg1, GeometryCombineMode.Exclude, null);

            _borderBackground.Clip = _borGeometry;

            GuideHintForWindow hit = new(this, point, targetControl, guide);
            hit.NextHintEvent -= Hit_NextHintEvent;
            hit.NextHintEvent += Hit_NextHintEvent;
            _canvasHint?.Children.Add(hit);
        }

        private void Hit_NextHintEvent()
        {
            while (true)
            {
                _canvasHint?.Children.Clear();
                if (_currentHintShowIndex >= _guideInfos.Count - 1)
                {
                    Close();
                    return;
                }

                _currentHintShowIndex++;

                GuideInfo currentGuideInfo = _guideInfos[_currentHintShowIndex];
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