using GraphX.Common;
using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.Logic.Models;
using Router.Enums;
using Router.Interfaces;
using Router.Model;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Router.Controls
{
    public class GraphAreaControl : GraphArea<Node, Link, Graph>
    {
        private bool _pendingLinkRequested;
        public static readonly DependencyProperty GraphControllerProperty = DependencyProperty.Register("GraphController", typeof(IGraphController), typeof(GraphAreaControl), new PropertyMetadata(null));
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

        public GraphAreaControl()
        {
            var logicCore = new GXLogicCore<Node, Link, Graph>(new Graph())
            {
                DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK,
                DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA,
                DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER,
            };
            logicCore.DefaultOverlapRemovalAlgorithmParams = logicCore.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            SetLogicCore(logicCore);
            SetVerticesHighlight(true, GraphControlType.VertexAndEdge);
            SetEdgesHighlight(true, GraphControlType.VertexAndEdge);
            GenerateGraph(LogicCore.Graph);

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            GraphController.GraphModeChanged += OnGraphModeChanged;
            GraphController.SetGraphMode(GraphMode.Select);

            GraphController.NodeRequested += AddNode;
            GraphController.LinkRequested += AddLink;
            GraphController.PendingLinkRequested += OnPendingLinkRequested;
            GraphController.PendingLinkCompleted += OnPendingLinkCompleted;
            VertexSelected += OnVertexSelected;
        }

        private void OnGraphModeChanged(GraphMode mode)
        {
            switch (mode)
            {
                case GraphMode.None:
                    break;
                case GraphMode.Select:
                    if (_pendingLinkRequested) GraphController.CompletePendingLink(null);
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
            _pendingLinkRequested = false;
            RemoveCustomChildControl(pendingLink.LinkPath);
        }

        private void OnPendingLinkRequested(PendingLink pendingLink)
        {
            _pendingLinkRequested = true;
            InsertCustomChildControl(0, pendingLink.LinkPath);
        }

        private void OnVertexSelected(object sender, VertexSelectedEventArgs args)
        {
            if (args.MouseArgs.LeftButton != MouseButtonState.Pressed) return;

            switch (GraphController.Mode)
            {
                case GraphMode.None:
                    break;
                case GraphMode.Select:
                    if (args.Modifiers == ModifierKeys.Control)
                    {
                        SwitchTagged(args.VertexControl);
                    }
                    break;
                case GraphMode.Edit:
                    var pos = args.MouseArgs.GetPosition(this);
                    var point = args.VertexControl.GetConnectionPointAt(pos);

                    if (point == null)
                    {
                        return;
                    }
                    else if (_pendingLinkRequested)
                    {
                        GraphController.CompletePendingLink(args.VertexControl);
                    }
                    else
                    {

                        GraphController.RequestPendingLink(args.VertexControl, pos.X, pos.Y);
                    }
                    break;
            }
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

        private void AddNode(Node node, double x, double y)
        {
            var nodeControl = new VertexControl(node);
            nodeControl.SetPosition(x, y);
            AddVertexAndData(node, nodeControl);
            nodeControl.OnApplyTemplate();
        }

        private void AddLink(Link link)
        {
            var sourceControl = VertexList[link.Source];
            var targetControl = VertexList[link.Target];

            var linkControl = new EdgeControl(sourceControl, targetControl, link);
            AddEdgeAndData(link, linkControl, true);
        }
    }
}
