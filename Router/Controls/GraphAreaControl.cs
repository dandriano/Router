using GraphX.Common;
using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using Router.Enums;
using Router.Interfaces;
using Router.Model;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Router.Controls
{
    public class GraphAreaControl : GraphArea<Node, Link, Network>
    {
        public GraphAreaControl()
        {
            SetVerticesHighlight(true, GraphControlType.VertexAndEdge);
            SetEdgesHighlight(true, GraphControlType.VertexAndEdge);

            Loaded += OnLoaded;
            VertexSelected += OnVertexSelected;
        }

        private void OnVertexSelected(object sender, VertexSelectedEventArgs args)
        {
            switch (((IGraphViewModel)DataContext).Mode)
            {
                case GraphMode.None:
                    break;
                case GraphMode.Select:
                    if (args.MouseArgs.LeftButton == MouseButtonState.Pressed)
                    {
                        if (args.Modifiers == ModifierKeys.Control)
                        {
                            SwitchTagged(args.VertexControl);
                        }
                    }
                    break;
                case GraphMode.Edit:
                    break;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ((IGraphViewModel)DataContext).GraphModeChanged += OnGraphModeChanged;
            ((IGraphViewModel)DataContext).NodeRequested += AddNode;
            ((IGraphViewModel)DataContext).LinkRequested += AddLink;
            ((IGraphViewModel)DataContext).PendingLinkRequested += OnPendingLinkRequested;
            ((IGraphViewModel)DataContext).PendingLinkCompleted += OnPendingLinkCompleted;

            var logic = ((IGraphViewModel)DataContext).LogicCore;
            SetLogicCore(logic);
            GenerateGraph(logic.Graph);
        }

        private void OnGraphModeChanged(GraphMode mode)
        {
            switch (mode)
            {
                case GraphMode.None:
                    break;
                case GraphMode.Select:
                    SetVerticesDrag(true, true);
                    SetEdgesDrag(true);
                    break;
                case GraphMode.Edit:
                    VertexList.Values
                        .Where(DragBehaviour.GetIsTagged)
                        .ForEach(SwitchTagged);

                    SetVerticesDrag(false, false);
                    SetEdgesDrag(false);
                    break;
            }
        }

        private void OnPendingLinkCompleted(PendingLink pendingLink)
        {
            RemoveCustomChildControl(pendingLink.LinkPath);
        }

        private void OnPendingLinkRequested(PendingLink pendingLink)
        {
            InsertCustomChildControl(0, pendingLink.LinkPath);
        }

        private void SwitchTagged(VertexControl nodeControl)
        {
            if (DragBehaviour.GetIsTagged(nodeControl))
            {
                HighlightBehaviour.SetHighlighted(nodeControl, false);
                DragBehaviour.SetIsTagged(nodeControl, false);
            }
            else
            {
                HighlightBehaviour.SetHighlighted(nodeControl, true);
                DragBehaviour.SetIsTagged(nodeControl, true);
            }
        }

        private void AddNode(Node node, Point pos)
        {
            var nodeControl = new NodeControl(node);
            pos = ((UIElement)Parent).TranslatePoint(pos, this);
            pos.Offset(-20, -20);
            nodeControl.SetPosition(pos.X, pos.Y);
            AddVertexAndData(node, nodeControl);
            nodeControl.OnApplyTemplate();
        }

        private void AddLink(Link link)
        {
            var sourceControl = VertexList[link.Source];
            var targetControl = VertexList[link.Target];

            var linkControl = new EdgeControl(sourceControl, targetControl, link);
            
            AddEdgeAndData(link, linkControl, true);
            UpdateAllEdges();
        }
    }
}
