using Dotnet9WPFControls.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dotnet9WPFControls.Converters
{
    public class BindControlToGuideCovnerter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                return null;
            }

            FrameworkElement? element = values[0] as FrameworkElement;
            GuideInfo? guide = values[1] as GuideInfo;
            guide!.Uc = element;

            return guide;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}