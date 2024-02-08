using Router.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Router.Misc
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(bool), typeof(GraphMode))]
    public class GraphModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is GraphMode p && value is GraphMode v) return v == p;
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is GraphMode g) return g;
            throw new NotImplementedException();
        }
    }
}
