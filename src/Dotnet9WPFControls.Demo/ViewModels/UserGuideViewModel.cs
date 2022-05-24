using Dotnet9WPFControls.Controls;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Input;

namespace Dotnet9WPFControls.Demo.ViewModels
{
    public class UserGuideViewModel : BindableBase
    {
        private GuideInfo? _borderSecurityGuide;
        private GuideInfo? _borderWalletOverviewGuide;
        private GuideInfo? _btnCloseGuide;
        private GuideInfo? _btnNotifactionGuide;
        private GuideInfo? _btnPayGuideGuide;
        private GuideInfo? _btnShowGuide;
        private GuideInfo? _btnWalletGuide;
        private ICommand? _showGuideCommand;

        public ICommand ShowGuideCommand =>
            _showGuideCommand ??= new DelegateCommand<List<object>>(GuideWindow.ShowGuideBox);

        public GuideInfo BtnShowGuide =>
            _btnShowGuide ??= new GuideInfo
            {
                Title = "点击这里显示新手引导", Content = "不知道如何操作那就点击这里查看新手引导吧", ButtonContent = "我知道了"
            };

        public GuideInfo BtnWalletGuide =>
            _btnWalletGuide ??= new GuideInfo { Title = "钱包详情在这", Content = "查看钱包余额、最近消费记录", ButtonContent = "我知道了" };

        public GuideInfo BtnNotifactionGuide =>
            _btnNotifactionGuide ??= new GuideInfo
            {
                Title = "实时消费提醒", Content = "监控实时消息、历史消费信息", ButtonContent = "我知道了"
            };

        public GuideInfo BorderWalletOverviewGuide =>
            _borderWalletOverviewGuide ??= new GuideInfo
            {
                Title = "账户概览信息", Content = "消费、支持、余额实时显示在这里", ButtonContent = "我知道了"
            };

        public GuideInfo BtnPayGuide =>
            _btnPayGuideGuide ??= new GuideInfo
            {
                Title = "交易在这里", Content = "输入对方钱包ID，然后点击支付即可完成交易", ButtonContent = "我知道了"
            };

        public GuideInfo BorderSecurityGuide =>
            _borderSecurityGuide ??= new GuideInfo
            {
                Title = "账户安全", Content = "这里修改您的安全信息，要慎重哦", ButtonContent = "我知道了"
            };

        public GuideInfo BtnCloseGuide =>
            _btnCloseGuide ??= new GuideInfo { Title = "点击这里关闭", Content = "暂时离别", ButtonContent = "我知道了" };
    }
}