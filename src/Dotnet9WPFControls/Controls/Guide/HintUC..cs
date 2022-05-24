using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace Dotnet9WPFControls.Controls
{
    [TemplatePart(Name = PartBackgroundViewbox, Type = typeof(Viewbox))]
    [TemplatePart(Name = PartBtnNext, Type = typeof(Button))]
    public class HintUc : Control
    {
        public delegate void NextHintDelegate();

        private const string PartBackgroundViewbox = "PART_Background_Viewbox";
        private const string PartBtnNext = "PART_Btn_Next";

        public static readonly DependencyProperty GridMarginProperty =
            DependencyProperty.Register(nameof(GridMargin), typeof(Thickness), typeof(HintUc),
                new PropertyMetadata(new Thickness(20, 30, 20, 20)));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(HintUc), new PropertyMetadata(null));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(string), typeof(HintUc), new PropertyMetadata(null));

        public static readonly DependencyProperty NextContentProperty =
            DependencyProperty.Register(nameof(NextContent), typeof(string), typeof(HintUc),
                new PropertyMetadata("下一步"));

        private readonly Window _ownerWindow;

        private readonly FrameworkElement _targetControl;

        private Viewbox? _backgroundViewbox;
        private Button? _btnNext;
        private Point _targetControlPoint;

        static HintUc()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HintUc), new FrameworkPropertyMetadata(typeof(HintUc)));
        }

        public HintUc(Window ownerWindow, Point point, FrameworkElement targetControl, GuideInfo guide)
        {
            _ownerWindow = ownerWindow;
            _targetControlPoint = point;
            _targetControl = targetControl;

            if (guide.Width != null)
            {
                Width = guide.Width.Value;
            }

            if (guide.Height != null)
            {
                Height = guide.Height.Value;
            }

            Title = guide.Title;
            Content = guide.Content;
            NextContent = guide.ButtonContent;

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

        public string? NextContent
        {
            get => (string?)GetValue(NextContentProperty);
            set => SetValue(NextContentProperty, value);
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
            if (leftOfTarget + ActualWidth <= _ownerWindow.Width &&
                bottomOfTarget + ActualHeight <= _ownerWindow.Height)
            {
                Canvas.SetLeft(this, leftOfTarget);
                Canvas.SetTop(this, bottomOfTarget);
            }
            // 2、提示框下侧会显示在蒙版外
            else if (leftOfTarget + ActualWidth <= _ownerWindow.Width &&
                     bottomOfTarget + ActualHeight > _ownerWindow.Height)
            {
                Canvas.SetLeft(this, leftOfTarget);
                Canvas.SetTop(this, topOfTarget - ActualHeight);

                ScaleTransform scaleTransform = new ScaleTransform {ScaleY = -1};
                _backgroundViewbox!.RenderTransform = scaleTransform;
                GridMargin = new Thickness(20, 20, 20, 30);
            }
            // 3、提示框右侧会显示在蒙版外
            else if (leftOfTarget + ActualWidth > _ownerWindow.Width &&
                     bottomOfTarget + ActualHeight <= _ownerWindow.Height)
            {
                Canvas.SetLeft(this, rightOfTarget - ActualWidth);
                Canvas.SetTop(this, bottomOfTarget);

                ScaleTransform scaleTransform = new ScaleTransform {ScaleX = -1};
                _backgroundViewbox!.RenderTransform = scaleTransform;
            }
            // 4、提示框右侧和下方会显示在蒙版外
            else if (leftOfTarget + ActualWidth > _ownerWindow.Width &&
                     bottomOfTarget + ActualHeight > _ownerWindow.Height)
            {
                Canvas.SetLeft(this, rightOfTarget - ActualWidth);
                Canvas.SetTop(this, topOfTarget - ActualHeight);

                ScaleTransform scaleTransform = new ScaleTransform {ScaleX = -1, ScaleY = -1};
                _backgroundViewbox!.RenderTransform = scaleTransform;
                GridMargin = new Thickness(20, 20, 20, 30);
            }
            else //怎么放都不行，就按第一种放吧
            {
                Canvas.SetLeft(this, rightOfTarget);
                Canvas.SetTop(this, bottomOfTarget);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_btnNext != null)
            {
                _btnNext.Click -= btn_next_Click;
            }

            _backgroundViewbox = GetTemplateChild(PartBackgroundViewbox) as Viewbox;
            _btnNext = GetTemplateChild(PartBtnNext) as Button;

            if (_btnNext != null)
            {
                _btnNext.Click += btn_next_Click;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            _btnNext?.Focus();
        }

        public event NextHintDelegate? NextHintEvent;

        private void btn_next_Click(object sender, RoutedEventArgs e)
        {
            NextHintEvent?.Invoke();
        }
    }
}