using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.Logic.Models;
using Router.Model;
using System.Windows;

namespace Router.ViewModels
{
    public class GraphViewModel : GraphArea<Node, Link, Graph>
    {
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

            VertexDoubleClick += ProceedVertexPointDoubleClick;
            // Uncomment for pre-creation of node pairs with link
            /*
            var n1 = AddNode(1, "#1 Node", new Point(25, 25));
            var n2 = AddNode(2, "#2 Node", new Point(100, 100));
            // var n3 = AddNode(3, "#3 Node", new Point(200, 200));
            var e = AddLink(n1, n2);
            */
            
        }

        private void ProceedVertexPointDoubleClick(object sender, VertexSelectedEventArgs args)
        {
            var nodeControl = args.VertexControl;
            var point = nodeControl.GetConnectionPointAt(args.MouseArgs.GetPosition(this));

            if (point != null)
            {
                // TODO: Add dynamic link creation on dblclick
            }
        }

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
    }
}
