﻿using Dotnet9WPFControls.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dotnet9WPFControls.Converters
{
    public class BindControlToGuideConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                return null;
            }

            var element = values[0] as FrameworkElement;
            var guide = values[1] as GuideInfo;
            if (guide != null)
            {
                guide.TargetControl = element;
            }

            return guide;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}