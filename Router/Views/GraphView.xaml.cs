using Router.ViewModels;
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
            GraphArea.AllowDrop = true;
            GraphArea.PreviewDrop += GraphArea_PreviewDrop;
            GraphArea.DragEnter += GraphArea_DragEnter;
        }

        private void GraphArea_DragEnter(object sender, DragEventArgs e)
        {
            //don't show drag effect if we are on drag source or don't have any item of needed type dragged
            if (!e.Data.GetDataPresent(typeof(object)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void GraphArea_PreviewDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(object))) return;
            //how to get dragged data by its type
            var pos = e.GetPosition(GraphArea);
            var graph = (GraphViewModel)DataContext;
            var id = graph.VertexList.Count + 1;
            graph.AddNode(id, $"#{id} Node", pos);
        }

        private void NewNodeDragSource_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var data = new DataObject(typeof(object), new object());
            DragDrop.DoDragDrop(NewNodeDragSource, data, DragDropEffects.Link);
        }
    }
}
