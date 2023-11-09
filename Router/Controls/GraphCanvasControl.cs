using GraphX.Controls;
using Router.Enums;
using Router.Interfaces;
using Router.Model;
using System.Windows;
using System.Windows.Input;

namespace Router.Controls
{
    public class GraphCanvasControl : ZoomControl
    {
        private MouseEventHandler _pendingLinkHandler;
        public static readonly DependencyProperty GraphControllerProperty = DependencyProperty.Register("GraphController", typeof(IGraphController), typeof(GraphCanvasControl), new PropertyMetadata(null));
        public IGraphController GraphController
        {
            get
            {
                return (IGraphController)GetValue(GraphControllerProperty);
            }
            set
            {
                SetValue(GraphControllerProperty, value);
            }
        }

        public GraphCanvasControl()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            MouseDown += OnMouseDown;

            GraphController.GraphModeChanged += GraphModeChanged;
            GraphController.SetGraphMode(GraphMode.Select);

            GraphController.PendingLinkRequested += OnPendingLinkRequested;
            GraphController.PendingLinkCompleted += OnPendingLinkCompleted;
        }

        private void OnPendingLinkCompleted(PendingLink pendingLink)
        {
            MouseMove -= _pendingLinkHandler;
            _pendingLinkHandler = null;
        }

        private void OnPendingLinkRequested(PendingLink pendingLink)
        {
            _pendingLinkHandler = (s, e) =>
            {
                var pos = TranslatePoint(e.GetPosition(this), (UIElement)Presenter.Content);
                pendingLink.UpdateTargetPosition(pos);
            };

            MouseMove += _pendingLinkHandler;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            switch (GraphController.Mode)
            {
                case GraphMode.None:
                    break;
                case GraphMode.Select:
                    break;
                case GraphMode.Edit:
                    var pos = TranslatePoint(e.GetPosition(this), (UIElement)Presenter.Content);
                    pos.Offset(-50, -50);
                    GraphController.AddNode("Moscow", pos.X, pos.Y);
                    break;
            }
        }

        private void GraphModeChanged(GraphMode mode)
        {
            switch (mode)
            {
                case GraphMode.None:
                    break;
                case GraphMode.Select:
                    Cursor = Cursors.Hand;
                    break;
                case GraphMode.Edit:
                    Cursor = Cursors.Pen;
                    break;
            }
        }
    }
}
