using GraphX.Controls;
using Router.Enums;
using Router.Model;
using System;

namespace Router.Interfaces
{
    public interface IGraphController
    {
        event Action<GraphMode> GraphModeChanged;
        event Action<Node, double, double> NodeRequested;
        event Action<Link> LinkRequested;
        event Action<PendingLink> PendingLinkRequested;
        event Action<PendingLink> PendingLinkCompleted;

        GraphMode Mode { get; }
        void SetGraphMode(GraphMode mode);
        void RequestPendingLink(VertexControl nodeControl, double x, double y);
        void CompletePendingLink(VertexControl nodeControl);
        Link AddLink(Node source, Node target, long weight = 1);
        Node AddNode(string name, double x, double y);
    }
}
