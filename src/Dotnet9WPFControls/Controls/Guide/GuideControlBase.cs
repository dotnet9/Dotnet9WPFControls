using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dotnet9WPFControls.Controls
{
    public class GuideControlBase
    {
        public const string PartBorderBackground = "PART_Border_Background";
        public const string PartCanvasHint = "PART_Canvas_Hint";

        public Border? BorderBackground;

        public PathGeometry BorGeometry = new();
        public Canvas? CanvasHint;

        public Action CloseHint;
        public int CurrentHintShowIndex;
        public List<GuideInfo> Guides;
        public Action<FrameworkElement?, GuideInfo> ShowHint;

        public GuideControlBase(Action closeHint, Action<FrameworkElement?, GuideInfo> showHint, List<GuideInfo> guides)
        {
            CloseHint = closeHint;
            ShowHint = showHint;
            Guides = guides;
        }

        public void ShowNextHint()
        {
            while (true)
            {
                CanvasHint?.Children.Clear();
                if (CurrentHintShowIndex >= Guides?.Count - 1)
                {
                    CloseHint!();
                    return;
                }

                CurrentHintShowIndex++;

                GuideInfo currentGuideInfo = Guides![CurrentHintShowIndex];
                if (currentGuideInfo.TargetControl == null)
                {
                    continue;
                }

                ShowHint(currentGuideInfo.TargetControl, currentGuideInfo);
                break;
            }
        }

        public void CombineHint(RectangleGeometry rg, FrameworkElement targetControl, Point targetControlPoint)
        {
            BorGeometry = Geometry.Combine(BorGeometry, rg, GeometryCombineMode.Union, null);
            BorderBackground!.Clip = BorGeometry;

            RectangleGeometry rg1 = new()
            {
                RadiusX = 3,
                RadiusY = 3,
                Rect = new Rect(targetControlPoint.X, targetControlPoint.Y, targetControl.ActualWidth,
                    targetControl.ActualHeight)
            };
            BorGeometry = Geometry.Combine(BorGeometry, rg1, GeometryCombineMode.Exclude, null);

            BorderBackground.Clip = BorGeometry;
        }
    }
}