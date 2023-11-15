using GraphX.Controls;
using Router.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Router.Controls
{
    public class NodeControl : VertexControl
    {
        public static readonly DependencyProperty ShowViewProperty =
                DependencyProperty.Register("ShowView", typeof(bool), typeof(NodeControl), new UIPropertyMetadata(null));

        public bool ShowView
        {
            get => (bool)GetValue(ShowViewProperty);
            set => SetValue(ShowViewProperty, value);
        }

        public static readonly DependencyProperty ViewVisibilityProperty =
                DependencyProperty.Register("ViewVisibility", typeof(Visibility), typeof(NodeControl), new UIPropertyMetadata(null));

        public Visibility ViewVisibility
        {
            get => (Visibility)GetValue(ViewVisibilityProperty);
            set => SetValue(ViewVisibilityProperty, value);
        }

        public static readonly DependencyProperty NodeTypeProperty =
                DependencyProperty.Register("NodeType", typeof(NodeType), typeof(NodeControl), new UIPropertyMetadata(null));

        public NodeType NodeType
        {
            get => (NodeType)GetValue(NodeTypeProperty);
            set => SetValue(NodeTypeProperty, value);
        }

        public static readonly DependencyProperty NodeImageProperty =
                DependencyProperty.Register("NodeImage", typeof(DrawingImage), typeof(NodeControl), new UIPropertyMetadata(null));

        public DrawingImage NodeImage
        {
            get => (DrawingImage)GetValue(NodeImageProperty);
            set => SetValue(NodeImageProperty, value);
        }

        public NodeControl(object vertexData, bool tracePositionChange = true, bool bindToDataObject = true) : base(vertexData, tracePositionChange, bindToDataObject)
        {
            ContextMenu = new ContextMenu();
            var menuItem = new MenuItem()
            {
                Header = "Edit"
            };
            menuItem.Click += (s, e) =>
            {
                ShowView = true;
            };
            ContextMenu.Items.Add(menuItem);
        }
    }
}
