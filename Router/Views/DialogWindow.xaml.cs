using Prism.Services.Dialogs;
using System.Windows;

namespace Router.Views
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, IDialogWindow
    {
        public IDialogResult Result { get; set; }

        public DialogWindow()
        {
            InitializeComponent();
        }
    }
}
