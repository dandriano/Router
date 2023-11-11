using GraphX.Common.Interfaces;
using Router.Enums;
using Router.Model;
using System;
using System.Windows;

namespace Router.Interfaces
{
    public interface IGraphViewModel
    {
        event Action<GraphMode> GraphModeChanged;
        event Action<Node, Point> NodeRequested;
        event Action<Link> LinkRequested;
        event Action<PendingLink> PendingLinkRequested;
        event Action<PendingLink> PendingLinkCompleted;

        GraphMode Mode { get; }
        IGXLogicCore<Node, Link, Graph> LogicCore { get; }
    }
}
