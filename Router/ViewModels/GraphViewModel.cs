using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.Logic.Models;
using Router.Model;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Router.ViewModels
{
    public class GraphViewModel : GraphArea<Node, Link, Graph>
    {
        private PendingLink PendingLink;

        public GraphViewModel()
        {
            var graph = new Graph();
            var logicCore = new GXLogicCore<Node, Link, Graph>(graph)
            {
                DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK,
                DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA,
                DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER,
            };
            logicCore.DefaultOverlapRemovalAlgorithmParams = logicCore.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            SetLogicCore(logicCore);
            GenerateGraph(graph);
            SetVerticesDrag(true, true);
            SetEdgesDrag(true);
            SetVerticesHighlight(true, GraphControlType.VertexAndEdge);
            SetEdgesHighlight(true, GraphControlType.VertexAndEdge);
            ShowAllEdgesArrows(true);

            VertexDoubleClick += StartPendingLink;
            MouseMove += MovePendingLink;
            VertexClicked += FinishPendingLink;
        }
        #region [Add Node/Link]
        public Node AddNode(long id, string name, Point pos)
        {
            var node = new Node(id, name);
            var nodeControl = new VertexControl(node);
            nodeControl.SetPosition(pos);
            AddVertexAndData(node, nodeControl);
            nodeControl.OnApplyTemplate();

            return node;
        }

        public Link AddLink(Node source, Node target, long weight = 1)
        {
            var sourceControl = VertexList[source];
            var targetControl = VertexList[target];
            var link = new Link(source, target, weight)
            {
                SourceConnectionPointId = 1,
                TargetConnectionPointId = 1
            };
            var linkControl = new EdgeControl(sourceControl, targetControl, link);
            AddEdgeAndData(link, linkControl, true);

            return link;
        }
        #endregion
        #region [Add PendingLink]
        private void StartPendingLink(object sender, VertexSelectedEventArgs args)
        {
            if (PendingLink != default) return;
            var nodeControl = args.VertexControl;
            var pos = args.MouseArgs.GetPosition(this);
            var point = nodeControl.GetConnectionPointAt(pos);

            if (point != null)
            {
                PendingLink = new PendingLink(nodeControl, point, pos, new SolidColorBrush(Colors.Blue));
                InsertCustomChildControl(0, PendingLink.LinkPath);
            }
        }

        private void MovePendingLink(object sender, MouseEventArgs e)
        {
            if (PendingLink == default) return;
            var pos = e.GetPosition(this);
            PendingLink.UpdateTargetPosition(pos);
        }

        private void FinishPendingLink(object sender, VertexClickedEventArgs args)
        {
            if (PendingLink == default) return;
            if (args.MouseArgs.ChangedButton != MouseButton.Right) return;

            var nodeControl = args.Control;
            var pos = args.MouseArgs.GetPosition(this);
            var point = nodeControl.GetConnectionPointAt(pos);

            if (point != null)
            {
                var source = (Node)PendingLink.Source.Vertex;
                var target = (Node)nodeControl.Vertex;

                RemoveCustomChildControl(PendingLink.LinkPath);
                PendingLink.Dispose();
                PendingLink = null;

                AddLink(source, target);
            }
        }
        #endregion
    }
}
