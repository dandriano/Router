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

        public GraphCanvasControl()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ((IGraphViewModel)DataContext).GraphModeChanged += GraphModeChanged;
            ((IGraphViewModel)DataContext).PendingLinkRequested += OnPendingLinkRequested;
            ((IGraphViewModel)DataContext).PendingLinkCompleted += OnPendingLinkCompleted;
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
