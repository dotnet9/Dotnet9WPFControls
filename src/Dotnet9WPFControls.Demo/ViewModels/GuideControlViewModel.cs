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

        public GuideInfo BtnCloseGuide =>
            _btnCloseGuide ??= new GuideInfo("关闭点这里", "不使用了点击这里关闭窗体");

        public GuideInfo BtnShowGuide =>
            _btnShowGuide ??= new GuideInfo("不会用？", "点击这个按钮显示引导");

        public GuideInfo BtnLeftTopGuide =>
            _btnLeftTopGuide ??= new GuideInfo("左上引导", "测试左上引导提示显示位置");

        public GuideInfo BtnLeftBottomGuide =>
            _btnLeftBottomGuide ??= new GuideInfo("左下引导", "测试左下引导提示显示位置");

        public GuideInfo BtnRightBottomGuide =>
            _btnRightBottomGuide ??= new GuideInfo("右下引导", "测试右下引导提示显示位置");

        public List<GuideInfo> GuideLists => new()
        {
            BtnShowGuide,
            BtnCloseGuide,
            BtnLeftTopGuide,
            BtnLeftBottomGuide,
            BtnRightBottomGuide
        };
    }
}