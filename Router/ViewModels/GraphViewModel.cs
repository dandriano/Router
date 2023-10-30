using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.Logic.Models;
using Router.Model;
using System;
using System.Windows;

namespace Router.ViewModels
{
    public class GraphViewModel : GraphArea<Node, Link, Graph>
    {
        public GraphViewModel()
        {
            VertexLabelFactory = new DefaultVertexlabelFactory();

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

            SetVerticesDrag(true);
            SetVerticesHighlight(true, GraphControlType.VertexAndEdge);
            ShowAllEdgesArrows(true);
            SetEdgesHighlight(true, GraphControlType.VertexAndEdge);
        }

        public Node AddNode(long id, string name)
        {
            var rnd = new Random();
            var position = new Point(rnd.Next(150), rnd.Next(150));

            var node = new Node(id, name);
            var nodeControl = new VertexControl(node);

            nodeControl.SetPosition(position);
            AddVertexAndData(node, nodeControl);

            return node;
        }

        public Link AddLink(Node source, Node target, long weight = 1)
        {
            var sourceControl = VertexList[source];
            var targetControl = VertexList[target];

            var link = new Link(source, target, weight);
            var linkControl = new EdgeControl(sourceControl, targetControl, link);

            AddEdgeAndData(link, linkControl);

            return link;
        }
    }
}
