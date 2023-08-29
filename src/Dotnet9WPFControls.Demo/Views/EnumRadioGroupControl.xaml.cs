using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System;
using System.Linq;
using System.Windows.Controls;

namespace Dotnet9WPFControls.Demo.Views
{
    public partial class EnumRadioGroupControl : UserControl
    {
        private readonly StackPanel _stackPanel;

        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register(nameof(SelectedValue), typeof(Enum), typeof(EnumRadioGroupControl),
                new PropertyMetadata(null, OnSelectedValueChanged));

        public Enum SelectedValue
        {
            get { return (Enum)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EnumRadioGroupControl control = (EnumRadioGroupControl)d;
            control.UpdateRadioButtons();
        }

        public EnumRadioGroupControl()
        {
            _stackPanel = new StackPanel();
            Content = _stackPanel;
        }

        private void UpdateRadioButtons()
        {
            if (SelectedValue == null)
                return;

            Type enumType = SelectedValue.GetType();
            if (!enumType.IsEnum)
                throw new ArgumentException("SelectedValue must be an enum type.");

            var enumValues = Enum.GetValues(enumType).Cast<Enum>();
            var radioButtons = enumValues.Select(enumValue =>
            {
                RadioButton radioButton = new()
                {
                    Content = GetEnumDescription(enumValue), IsChecked = enumValue.Equals(SelectedValue)
                };
                radioButton.Checked += RadioButton_Checked;
                radioButton.Tag = enumValue;
                return radioButton;
            }).ToList();

            // Clear existing radio buttons
            _stackPanel.Children.Clear();

            // Add new radio buttons
            foreach (var radioButton in radioButtons)
            {
                _stackPanel.Children.Add(radioButton);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            SelectedValue = (Enum)radioButton.Tag;
        }

        private string GetEnumDescription(Enum enumValue)
        {
            FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            DescriptionAttribute[]? attributes =
                fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return attributes?.Length > 0 ? attributes[0].Description : enumValue.ToString();
        }
    }
}