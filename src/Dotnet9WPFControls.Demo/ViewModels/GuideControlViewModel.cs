using Dotnet9WPFControls.Controls;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Dotnet9WPFControls.Demo.ViewModels
{
    public class GuideControlViewModel : BindableBase
    {
        private GuideInfo? _btnCloseGuide;

        private GuideInfo? _btnLeftBottomGuide;

        private GuideInfo? _btnLeftTopGuide;

        private GuideInfo? _btnRightBottomGuide;

        private GuideInfo? _btnShowGuide;

        private GuideInfo? _listBoxItemGuide;

        public GuideInfo BtnCloseGuide =>
            _btnCloseGuide ??= new GuideInfo("关闭点这里", "不使用了点击这里关闭窗体");

        public GuideInfo BtnShowGuide =>
            _btnShowGuide ??= new GuideInfo("不会用？点击这里吧，可以查看引导在窗体不同位置的显示", "窗体的4个角、中间展示引导提示信息，标题或者描述较长以换行的形式展示");

        public GuideInfo BtnLeftTopGuide =>
            _btnLeftTopGuide ??= new GuideInfo("左上引导", "测试左上引导提示显示位置");

        public GuideInfo BtnLeftBottomGuide =>
            _btnLeftBottomGuide ??= new GuideInfo("左下引导", "测试左下引导提示显示位置");

        public GuideInfo BtnRightBottomGuide =>
            _btnRightBottomGuide ??= new GuideInfo("右下引导", "测试右下引导提示显示位置");

        public GuideInfo ListBoxItemGuide =>
            _listBoxItemGuide ??= new GuideInfo("嵌套控件的引导", "测试嵌套控件的引导");

        public List<GuideInfo> GuideLists => new()
        {
            BtnShowGuide,
            BtnCloseGuide,
            BtnLeftTopGuide,
            BtnLeftBottomGuide,
            BtnRightBottomGuide,
            ListBoxItemGuide
        };
    }
}