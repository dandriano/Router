using System.Windows;
using System.Windows.Controls;

namespace Router.Views
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        public GraphView()
        {
            InitializeComponent();
            NewNodeDragSource.PreviewMouseLeftButtonDown += NewNodeDragSource_PreviewMouseLeftButtonDown;
        }

        private void NewNodeDragSource_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var data = new DataObject(typeof(object), new object());
            DragDrop.DoDragDrop(NewNodeDragSource, data, DragDropEffects.Link);
        }
    }
}
