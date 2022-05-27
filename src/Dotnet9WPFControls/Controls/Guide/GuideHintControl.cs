using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dotnet9WPFControls.Controls
{
    [TemplatePart(Name = PartBtnClose, Type = typeof(Button))]
    [TemplatePart(Name = PartBackgroundViewbox, Type = typeof(Viewbox))]
    [TemplatePart(Name = PartBtnNext, Type = typeof(Button))]
    public class GuideHintControl : Control
    {
        public delegate void NextHintDelegate();

        private const string PartBtnClose = "PART_Btn_Close";
        private const string PartBackgroundViewbox = "PART_Background_Viewbox";
        private const string PartBtnNext = "PART_Btn_Next";

        public static readonly DependencyProperty GridMarginProperty =
            DependencyProperty.Register(nameof(GridMargin), typeof(Thickness), typeof(GuideHintControl),
                new PropertyMetadata(new Thickness(16, 26, 16, 16)));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(GuideHintControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(string), typeof(GuideHintControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ButtonContentProperty =
            DependencyProperty.Register(nameof(ButtonContent), typeof(string), typeof(GuideHintControl),
                new PropertyMetadata(""));

        private readonly FrameworkElement _targetControl;

        protected readonly FrameworkElement OwnerContainer;
        private Viewbox? _backgroundViewbox;

        private Button? _btnClose;
        private Button? _btnNext;
        private Point _targetControlPoint;

        public Action? CloseHint;

        static GuideHintControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GuideHintControl),
                new FrameworkPropertyMetadata(typeof(GuideHintControl)));
        }

        public GuideHintControl(FrameworkElement ownerContainer, Point targetControlPoint,
            FrameworkElement targetControl,
            GuideInfo guide, Action closeHint)
        {
            OwnerContainer = ownerContainer;
            _targetControlPoint = targetControlPoint;
            _targetControl = targetControl;

            MinWidth = guide.MinWidth;
            MinHeight = guide.MinHeight;
            Title = guide.Title;
            Content = guide.Content;
            ButtonContent = guide.ButtonContent;
            CloseHint = closeHint;

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
            double leftOfTarget = _targetControlPoint.X;
            double rightOfTarget = _targetControlPoint.X + _targetControl.ActualWidth;
            double rightOfOwnerHint = _targetControlPoint.X + ActualWidth;
            double topOfTarget = _targetControlPoint.Y;
            double bottomOfTarget = _targetControlPoint.Y + _targetControl.ActualHeight;
            double bottomOfOwnerHint = _targetControlPoint.Y + ActualHeight;

            // 1、正常情况：引导框左上角显示在该控件左下角
            if (leftOfTarget + ActualWidth <= OwnerContainer.ActualWidth &&
                bottomOfTarget + ActualHeight <= OwnerContainer.ActualHeight)
            {
                Canvas.SetLeft(this, leftOfTarget);
                Canvas.SetTop(this, bottomOfTarget);
            }
            // 2、提示框下侧会显示在蒙版外
            else if (leftOfTarget + ActualWidth <= OwnerContainer.ActualWidth &&
                     bottomOfTarget + ActualHeight > OwnerContainer.ActualHeight)
            {
                Canvas.SetLeft(this, leftOfTarget);
                Canvas.SetTop(this, topOfTarget - ActualHeight);

                ScaleTransform scaleTransform = new() {ScaleY = -1};
                _backgroundViewbox!.RenderTransform = scaleTransform;
                GridMargin = new Thickness(16, 16, 16, 26);
            }
            // 3、提示框右侧会显示在蒙版外
            else if (leftOfTarget + ActualWidth > OwnerContainer.ActualWidth &&
                     bottomOfTarget + ActualHeight <= OwnerContainer.ActualHeight)
            {
                Canvas.SetLeft(this, rightOfTarget - ActualWidth);
                Canvas.SetTop(this, bottomOfTarget);

                ScaleTransform scaleTransform = new() {ScaleX = -1};
                _backgroundViewbox!.RenderTransform = scaleTransform;
            }
            // 4、提示框右侧和下方会显示在蒙版外
            else if (leftOfTarget + ActualWidth > OwnerContainer.ActualWidth &&
                     bottomOfTarget + ActualHeight > OwnerContainer.ActualHeight)
            {
                Canvas.SetLeft(this, rightOfTarget - ActualWidth);
                Canvas.SetTop(this, topOfTarget - ActualHeight);

                ScaleTransform scaleTransform = new() {ScaleX = -1, ScaleY = -1};
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
            CloseHint?.Invoke();
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