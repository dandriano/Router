using Router.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Router.Misc
{
    [ValueConversion(typeof(bool), typeof(NodeType))]
    public class NodeModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is NodeType p && value is NodeType v) return v == p;
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is NodeType g) return g;
            throw new NotImplementedException();
        }
    }
}
