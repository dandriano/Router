using MaterialDesignThemes.Wpf;
using Router.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Router.Misc
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(GraphMode), typeof(DrawerHostOpenMode))]
    public class OpenModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (GraphMode)value;

            return v == GraphMode.Edit ? DrawerHostOpenMode.Modal : DrawerHostOpenMode.Standard;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
