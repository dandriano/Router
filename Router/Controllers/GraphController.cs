using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.Logic.Models;
using Router.Interfaces;
using Router.Model;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Router.Controllers
{
    public class GraphController : GraphArea<Node, Link, Graph>, IGraphController
    {
        private PendingLink PendingLink;
        public Graph Graph => LogicCore.Graph;

        public GraphController()
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
        }

        public void Initialize()
        {
            SetVerticesDrag(true, true);
            SetEdgesDrag(true);
            SetVerticesHighlight(true, GraphControlType.VertexAndEdge);
            SetEdgesHighlight(true, GraphControlType.VertexAndEdge);
            ShowAllEdgesArrows(true);

            VertexDoubleClick += StartPendingLink;
            VertexClicked += FinishPendingLink;

            (Parent as ZoomControl).AllowDrop = true;
            (Parent as ZoomControl).PreviewDrop += NewElementPreviewDrop;
            (Parent as ZoomControl).DragEnter += NewElementDragEnter;
            (Parent as ZoomControl).MouseMove += MovePendingLink;
            (Parent as ZoomControl).MouseRightButtonDown += FinishPendingLink;

            GenerateGraph(LogicCore.Graph);
        }

        #region [Add Node/Link]
        public Node AddNode(long id, string name, double x, double y)
        {
            var node = new Node(id, name);
            var nodeControl = new VertexControl(node);
            nodeControl.SetPosition(x, y);
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
        private void InitPendingLink(VertexControl nodeControl, IVertexConnectionPoint point, Point pos)
        {
            if (point != null)
            {
                PendingLink = new PendingLink(nodeControl, point, pos, new SolidColorBrush(Colors.Blue));
                InsertCustomChildControl(0, PendingLink.LinkPath);
            }
        }
        private void StartPendingLink(object sender, VertexSelectedEventArgs args)
        {
            if (PendingLink != default) return;
            var nodeControl = args.VertexControl;
            var pos = args.MouseArgs.GetPosition(Parent as ZoomControl);
            var point = nodeControl.GetConnectionPointAt(pos);

            InitPendingLink(nodeControl, point, pos);
        }

        private void MovePendingLink(object sender, MouseEventArgs e)
        {
            if (PendingLink == default) return;
            var pos = e.GetPosition(Parent as ZoomControl);
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

        private void FinishPendingLink(object sender, MouseButtonEventArgs e)
        {
            if (PendingLink == default) return;
            if (e.ChangedButton != MouseButton.Right) return;
            var pos = e.GetPosition(Parent as ZoomControl);
            var id = GetNextUniqueId(true);
            var newNode = AddNode(id, $"#{id} Node", pos.X, pos.Y);
            var nodeControl = VertexList[newNode];
            var point = nodeControl.GetConnectionPointById(1);

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
        #region [Add Node/Link From "Stencils" callbacks]
        private void NewElementDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Enums.EnumStencilDragType)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void NewElementPreviewDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Enums.EnumStencilDragType))) return;
            var whatDrop = (Enums.EnumStencilDragType)e.Data.GetData(typeof(Enums.EnumStencilDragType));
            var pos = e.GetPosition(this);

            switch (whatDrop)
            {
                case Enums.EnumStencilDragType.None:
                    break;
                case Enums.EnumStencilDragType.Node:
                    {
                        var id = GetNextUniqueId(true);
                        AddNode(id, $"#{id} Node", pos.X, pos.Y);
                    }
                    break;
                case Enums.EnumStencilDragType.Link:
                    var nodeControl = GetVertexControlAt(pos);
                    if (nodeControl == null)
                    {
                        var id = GetNextUniqueId(true);
                        var newNode = AddNode(id, $"#{id} Node", pos.X, pos.Y);
                        nodeControl = VertexList[newNode];
                    }
                    var point = nodeControl.GetConnectionPointById(1);
                    InitPendingLink(nodeControl, point, pos);
                    break;
            }
        }
        #endregion
    }
}
