using Router.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Router.Misc
{
    [ValueConversion(typeof(bool), typeof(LinkType))]
    public class LinkModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is LinkType p && value is LinkType v) return v == p;
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is LinkType g) return g;
            throw new NotImplementedException();
        }
    }
}
