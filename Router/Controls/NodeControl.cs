using GraphX.Controls;
using System.Windows;

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

        public NodeControl(object vertexData, bool tracePositionChange = true, bool bindToDataObject = true) : base(vertexData, tracePositionChange, bindToDataObject) { }
    }
}
