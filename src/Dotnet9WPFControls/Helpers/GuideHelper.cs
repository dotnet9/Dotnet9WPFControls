using Dotnet9WPFControls.Controls;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Dotnet9WPFControls.Helpers
{
    public static class GuideHelper
    {
        public static readonly DependencyProperty GuideInfoProperty
            = DependencyProperty.RegisterAttached("GuideInfo",
                typeof(GuideInfo),
                typeof(GuideHelper),
                new UIPropertyMetadata(default(GuideInfo)));

        public static ICommand ShowGuideWindowCommand { get; } =
            new DelegateCommand<object>(ExecuteShowGuideWindowCommand);

        public static GuideInfo? GetGuideInfo(UIElement dependencyObject)
        {
            return (GuideInfo)dependencyObject.GetValue(GuideInfoProperty);
        }

        public static void SetGuideInfo(UIElement dependencyObject, GuideInfo? guideInfo)
        {
            dependencyObject.SetValue(GuideInfoProperty, guideInfo);
        }


        public static void ExecuteShowGuideWindowCommand(object guide)
        {
            List<GuideInfo>? guideList;
            if (guide.GetType() == typeof(GuideInfo))
            {
                guideList = new List<GuideInfo> { (GuideInfo)guide };
            }
            else if (guide.GetType() == typeof(List<GuideInfo>))
            {
                guideList = (List<GuideInfo>)guide;
            }
            else
            {
                throw new Exception($"引导参数不正确，应该为 {typeof(GuideInfo)} 或者 {typeof(List<GuideInfo>)}");
            }

            Window? ownerWindow = Window.GetWindow(guideList[0].TargetControl!);
            if (ownerWindow == null)
            {
                return;
            }

            GuideWindow win = new GuideWindow(Window.GetWindow(guideList[0].TargetControl!)!, guideList);

            win.Show();
        }
    }
}