using Dotnet9WPFControls.Controls;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Dotnet9WPFControls.Demo.ViewModels
{
    public class GuideWindowViewModel : BindableBase
    {
        private GuideInfo? _borderSecurityGuide;
        private GuideInfo? _borderWalletOverviewGuide;
        private GuideInfo? _btnAddFilterGuide;
        private GuideInfo? _btnCloseGuide;
        private GuideInfo? _btnEditInfoGuide;
        private GuideInfo? _btnNotifactionGuide;
        private GuideInfo? _btnPayGuide;
        private GuideInfo? _btnRecordGuide;
        private GuideInfo? _btnShowGuide;
        private GuideInfo? _btnWalletGuide;
        private GuideInfo? _txtPayCodeGuide;

        public GuideInfo BtnWalletGuide =>
            _btnWalletGuide ??= new GuideInfo("钱包详情在这", "查看钱包余额、最近消费记录");


        public GuideInfo BtnNotifactionGuide =>
            _btnNotifactionGuide ??= new GuideInfo("实时消费提醒", "监控实时消息、历史消费信息");

        public GuideInfo BtnRecordGuide =>
            _btnRecordGuide ??= new GuideInfo("随手记录", "自定义记录，记录更多");

        public GuideInfo BtnCloseGuide =>
            _btnCloseGuide ??= new GuideInfo("点击这里关闭", "暂时离别");

        public GuideInfo BorderWalletOverviewGuide =>
            _borderWalletOverviewGuide ??= new GuideInfo("账户概览信息", "消费、支持、余额实时显示在这里");

        public GuideInfo TxtPayCodeGuide =>
            _txtPayCodeGuide ??= new GuideInfo("钱包ID", "请再三确认钱包ID，再点击下面的支持按钮哦");

        public GuideInfo BtnPayGuide =>
            _btnPayGuide ??= new GuideInfo("交易在这里", "上面交易信息确认无误，点击这里完成交易");

        public GuideInfo BtnAddFilterGuide =>
            _btnAddFilterGuide ??= new GuideInfo("添加自定义的过滤信息", "不满足预设过滤条件，自己掌控所有");

        public GuideInfo BtnEditInfoGuide =>
            _btnEditInfoGuide ??= new GuideInfo("修改钱包信息", "钱包信息有误，点这里修改");

        public GuideInfo BorderSecurityGuide =>
            _borderSecurityGuide ??= new GuideInfo("账户安全", "这里修改您的安全信息，要慎重哦");

        public GuideInfo BtnShowGuide =>
            _btnShowGuide ??= new GuideInfo("点击这里显示新手引导", "不知道如何操作那就点击这里查看新手引导吧");

        public List<GuideInfo> GuideLists => new()
        {
            BtnWalletGuide,
            BtnNotifactionGuide,
            BtnRecordGuide,
            BtnCloseGuide,
            BorderWalletOverviewGuide,
            TxtPayCodeGuide,
            BtnPayGuide,
            BtnAddFilterGuide,
            BtnEditInfoGuide,
            BorderSecurityGuide,
            BtnShowGuide
        };
    }
}