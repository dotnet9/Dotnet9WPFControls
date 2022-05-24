using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Dotnet9WPFControls.Controls.Helpers
{
    public static class Commands
    {
        public static ICommand ShowGuideCommand { get; } =
            new DelegateCommand<object>(ExecuteShowGuideCommand);


        public static void ExecuteShowGuideCommand(object guide)
        {
            List<GuideInfo>? guideList = null;
            if (guide.GetType() == typeof(GuideInfo))
            {
                guideList = new List<GuideInfo> {(GuideInfo)guide};
            }
            else if (guide.GetType() == typeof(List<GuideInfo>))
            {
                guideList = (List<GuideInfo>)guide;
            }
            else
            {
                throw new Exception($"引导参数不正确，应该为 {typeof(GuideInfo)} 或者 {typeof(List<GuideInfo>)}");
            }

            GuideWindow win = new(Window.GetWindow(guideList[0].Uc!)!, guideList);

            win.ShowDialog();
        }
    }
}