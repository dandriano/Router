using System.Windows;
using System.Windows.Controls;

namespace Router.Controls
{
    /// <summary>
    /// Расширение текстбокса для отображения текста по умолчанию
    /// </summary>
    public class DefaultTextTextBox : TextBox
    {
        //ссылки:
        //https://docs.microsoft.com/en-us/dotnet/desktop/wpf/controls/creating-a-control-that-has-a-customizable-appearance?view=netframeworkdesktop-4.8
        //https://docs.microsoft.com/en-us/dotnet/desktop/wpf/properties/custom-dependency-properties?view=netdesktop-6.0
        //https://stackoverflow.com/questions/51866779/setting-default-controltemplate-for-wpf-customcontrol

        //регистрируем новое свойство - отображаемый текст по умолчанию
        public static readonly DependencyProperty DefaultTextProperty =
        DependencyProperty.Register("DefaultText", typeof(string), typeof(DefaultTextTextBox), new UIPropertyMetadata(null));

        //property wrapper для нового свойства
        public string DefaultText
        {
            get => (string)GetValue(DefaultTextProperty);
            set => SetValue(DefaultTextProperty, value);
        }
    }
}
