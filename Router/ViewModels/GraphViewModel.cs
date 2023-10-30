using GraphX.Common.Enums;
using GraphX.Controls;
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

            /*
            var n1 = AddNode(1, "#1 Node", new Point(25, 25));
            var n2 = AddNode(2, "#2 Node", new Point(100, 100));
            var e = AddLink(n1, n2);
            */
        }

        public Node AddNode(long id, string name, Point pos)
        {
            var node = new Node(id, name);
            var nodeControl = new VertexControl(node);
            /*
            var vcp = new StaticVertexConnectionPoint() { Id = 1 };
            var ctl = new Border() { Child = vcp };
            nodeControl.VCPRoot.Children.Add(ctl);
            nodeControl.VertexConnectionPointsList.Add(vcp);
            */
            nodeControl.SetPosition(pos);
            AddVertexAndData(node, nodeControl);

            return node;
        }

        public Link AddLink(Node source, Node target, long weight = 1)
        {
            var sourceControl = VertexList[source];
            var targetControl = VertexList[target];

            var link = new Link(source, target, weight);
            var linkControl = new EdgeControl(sourceControl, targetControl, link);
            AddEdgeAndData(link, linkControl, true);

            return link;
        }
    }
}
