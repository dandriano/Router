using Router.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            NewLinkDragSource.PreviewMouseLeftButtonDown += NewLinkDragSource_PreviewMouseLeftButtonDown;
        }

        private void NewLinkDragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(NewLinkDragSource, EnumStencilDragType.Link, DragDropEffects.Link);
        }

        private void NewNodeDragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(NewNodeDragSource, EnumStencilDragType.Node, DragDropEffects.Link);
        }
    }
}
