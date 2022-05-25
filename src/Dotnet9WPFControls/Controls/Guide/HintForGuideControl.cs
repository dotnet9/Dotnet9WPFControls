using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    [TemplatePart(Name = PartBtnClose, Type = typeof(Button))]
    [TemplatePart(Name = PartBackgroundViewbox, Type = typeof(Viewbox))]
    [TemplatePart(Name = PartBtnNext, Type = typeof(Button))]
    public class HintForGuideControl : Control
    {
        public delegate void NextHintDelegate();

        private const string PartBtnClose = "PART_Btn_Close";
        private const string PartBackgroundViewbox = "PART_Background_Viewbox";
        private const string PartBtnNext = "PART_Btn_Next";

        public static readonly DependencyProperty GridMarginProperty =
            DependencyProperty.Register(nameof(GridMargin), typeof(Thickness), typeof(HintForGuideControl),
                new PropertyMetadata(new Thickness(16, 26, 16, 16)));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(HintForGuideControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(string), typeof(HintForGuideControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register(nameof(ButtonContent), typeof(string), typeof(HintForGuideControl),
                new PropertyMetadata("下一步"));

        private readonly FrameworkElement _ownerContainer;

        private readonly FrameworkElement _targetControl;
        private Viewbox? _backgroundViewbox;

        private Button? _btnClose;
        private Button? _btnNext;
        private Point _targetControlPoint;

        static HintForGuideControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintForGuideControl),
                new FrameworkPropertyMetadata(typeof(HintForGuideControl)));
        }

        public HintForGuideControl(FrameworkElement ownerContainer, Point point, FrameworkElement targetControl,
            GuideInfo guide)
        {
            _ownerContainer = ownerContainer;
            _targetControlPoint = point;
            _targetControl = targetControl;

            MinWidth = guide.MinWidth;
            MinHeight = guide.MinHeight;
            Title = guide.Title;
            Content = guide.Content;
            ButtonContent = guide.ButtonContent;

            Loaded += HintUC_Loaded;
        }

        public string? Title
        {
            get => (string?)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string? Content
        {
            get => (string?)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public string ButtonContent
        {
            get => (string)GetValue(ButtonContentProperty);
            set => SetValue(ButtonContentProperty, value);
        }

        public Thickness GridMargin
        {
            get => (Thickness)GetValue(GridMarginProperty);
            set => SetValue(GridMarginProperty, value);
        }

        private void HintUC_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= HintUC_Loaded;
            double leftOfTarget = _targetControlPoint.X - 5;
            double rightOfTarget = _targetControlPoint.X + _targetControl.ActualWidth + 5;
            double rightOfOwnerHint = _targetControlPoint.X + ActualWidth + 5;
            double topOfTarget = _targetControlPoint.Y - 10;
            double bottomOfTarget = _targetControlPoint.Y + _targetControl.ActualHeight + 10;
            double bottomOfOwnerHint = _targetControlPoint.Y + ActualHeight - 10;

            // 1、正常情况：引导框左上角显示在该控件左下角
            if (leftOfTarget + ActualWidth <= _ownerContainer.ActualWidth &&
                bottomOfTarget + ActualHeight <= _ownerContainer.ActualHeight)
            {
                Canvas.SetLeft(this, leftOfTarget);
                Canvas.SetTop(this, bottomOfTarget);
            }
            // 2、提示框下侧会显示在蒙版外
            else if (leftOfTarget + ActualWidth <= _ownerContainer.ActualWidth &&
                     bottomOfTarget + ActualHeight > _ownerContainer.ActualHeight)
            {
                Canvas.SetLeft(this, leftOfTarget);
                Canvas.SetTop(this, topOfTarget - ActualHeight);

                ScaleTransform scaleTransform = new() { ScaleY = -1 };
                _backgroundViewbox!.RenderTransform = scaleTransform;
                GridMargin = new Thickness(16, 16, 16, 26);
            }
            // 3、提示框右侧会显示在蒙版外
            else if (leftOfTarget + ActualWidth > _ownerContainer.ActualWidth &&
                     bottomOfTarget + ActualHeight <= _ownerContainer.ActualHeight)
            {
                Canvas.SetLeft(this, rightOfTarget - ActualWidth);
                Canvas.SetTop(this, bottomOfTarget);

                ScaleTransform scaleTransform = new() { ScaleX = -1 };
                _backgroundViewbox!.RenderTransform = scaleTransform;
            }
            // 4、提示框右侧和下方会显示在蒙版外
            else if (leftOfTarget + ActualWidth > _ownerContainer.ActualWidth &&
                     bottomOfTarget + ActualHeight > _ownerContainer.ActualHeight)
            {
                Canvas.SetLeft(this, rightOfTarget - ActualWidth);
                Canvas.SetTop(this, topOfTarget - ActualHeight);

                ScaleTransform scaleTransform = new() { ScaleX = -1, ScaleY = -1 };
                _backgroundViewbox!.RenderTransform = scaleTransform;
                GridMargin = new Thickness(16, 16, 16, 26);
            }
            else //怎么放都不行，就按第一种放吧
            {
                Canvas.SetLeft(this, leftOfTarget);
                Canvas.SetTop(this, bottomOfTarget);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_btnNext != null)
            {
                _btnNext.Click -= BtnNext_Click;
            }

            _btnClose = GetTemplateChild(PartBtnClose) as Button;
            _backgroundViewbox = GetTemplateChild(PartBackgroundViewbox) as Viewbox;
            _btnNext = GetTemplateChild(PartBtnNext) as Button;

            if (_btnClose != null)
            {
                _btnClose.Click += BtnClose_Click;
            }

            if (_btnNext != null)
            {
                _btnNext.Click += BtnNext_Click;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            (_ownerContainer as GuideControl)?.HideGuide();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            _btnNext?.Focus();
        }

        public event NextHintDelegate? NextHintEvent;

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            NextHintEvent?.Invoke();
        }
    }
}