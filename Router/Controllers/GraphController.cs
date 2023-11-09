using GraphX.Controls;
using Router.Enums;
using Router.Interfaces;
using Router.Model;
using System;
using System.Windows.Media;

namespace Router.Controllers
{
    public class GraphController : IGraphController
    {
        private PendingLink _pendingLink;
        public event Action<GraphMode> GraphModeChanged;
        public event Action<Node, double, double> NodeRequested;
        public event Action<Link> LinkRequested;
        public event Action<PendingLink> PendingLinkRequested;
        public event Action<PendingLink> PendingLinkCompleted;

        public GraphMode Mode { get; private set; }

        public void SetGraphMode(GraphMode mode)
        {
            Mode = mode;
            GraphModeChanged?.Invoke(mode);
        }

        public void RequestPendingLink(VertexControl nodeControl, double x, double y)
        {
            _pendingLink = new PendingLink(nodeControl, x, y, new SolidColorBrush(Colors.Blue));
            PendingLinkRequested?.Invoke(_pendingLink);
        }

        public void CompletePendingLink(VertexControl nodeControl)
        {
            _pendingLink.SetTarget(nodeControl);
            PendingLinkCompleted?.Invoke(_pendingLink);

            AddLink(_pendingLink.Source, _pendingLink.Target);
            _pendingLink = null;
        }

        public Link AddLink(Node source, Node target, long weight = 1)
        {
            if (source == null || target == null) return null;

            var link = new Link(source, target, weight);
            LinkRequested?.Invoke(link);

            return link;
        }

        public Node AddNode(string name, double x, double y)
        {
            var node = new Node(name);
            NodeRequested?.Invoke(node, x, y);

            return node;
        }
    }
}
