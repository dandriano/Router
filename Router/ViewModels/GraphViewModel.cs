using GraphX.Common.Enums;
using GraphX.Common.Interfaces;
using GraphX.Controls.Models;
using GraphX.Logic.Models;
using Prism.Commands;
using Prism.Mvvm;
using Router.Enums;
using Router.Interfaces;
using Router.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace Router.ViewModels
{
    public class GraphViewModel : BindableBase, IGraphViewModel
    {
        #region [Fields and Properties]
        private PendingLink _pendingLink;
        public IGXLogicCore<Node, Link, Network> LogicCore { get; private set; }
        #region [Commands and Events]
        public DelegateCommand Initialize { get; private set; }
        public DelegateCommand<VertexSelectedEventArgs> NodeSelected { get; private set; }
        public DelegateCommand<MouseButtonEventArgs> CanvasInteraction { get; private set; }

        public event Action<GraphMode> GraphModeChanged;
        public event Action<Node, Point> NodeRequested;
        public event Action<Link> LinkRequested;
        public event Action<PendingLink> PendingLinkRequested;
        public event Action<PendingLink> PendingLinkCompleted;
        #endregion
        #region [Observables]
        public ObservableCollection<Node> Nodes { get; private set; } = new ObservableCollection<Node>();
        public ObservableCollection<Link> Links { get; private set; } = new ObservableCollection<Link>();

        private dynamic _selectedElement;
        public dynamic SelectedElement
        {
            get => _selectedElement;
            set => SetProperty(ref _selectedElement, value);
        }

        private GraphMode _mode;
        public GraphMode Mode
        {
            get => _mode;
            set => SetProperty(ref _mode, value, () =>
            {
                if (value != GraphMode.Edit && _pendingLink != default)
                {
                    PendingLinkCompleted?.Invoke(_pendingLink);
                    _pendingLink = null;
                }
                GraphModeChanged?.Invoke(value);
            });
        }

        private NodeType _nodeMode;
        public NodeType NodeMode
        {
            get => _nodeMode;
            set => SetProperty(ref _nodeMode, value);
        }

        private LinkType _linkMode;
        public LinkType LinkMode
        {
            get => _linkMode;
            set => SetProperty(ref _linkMode, value);
        }

        private bool _inDrawerViewMode;
        public bool InDrawerViewMode
        {
            get => _inDrawerViewMode;
            set => SetProperty(ref _inDrawerViewMode, value);
        }
        #endregion
        #endregion
        public GraphViewModel()
        {
            LogicCore = new GXLogicCore<Node, Link, Network>(new Network())
            {
                DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK,
                DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA,
                DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER,
                EnableParallelEdges = true,
                ParallelEdgeDistance = 25,
            };
            LogicCore.DefaultOverlapRemovalAlgorithmParams = LogicCore.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);
            LogicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            LogicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            Mode = GraphMode.Select;
            NodeMode = NodeType.Terminal;
            LinkMode = LinkType.Simplex;

            NodeSelected = new DelegateCommand<VertexSelectedEventArgs>(OnNodeSelected);
            CanvasInteraction = new DelegateCommand<MouseButtonEventArgs>(OnCanvasInteraction);
        }

        private void OnCanvasInteraction(MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            switch (Mode)
            {
                case GraphMode.Select:
                    InDrawerViewMode = false;
                    break;
                case GraphMode.Edit:
                    var pos = e.GetPosition((UIElement)e.Source);
                    var node = AddNode($"#{Nodes.Count + 1} Node", NodeMode);

                    NodeRequested.Invoke(node, pos);
                    SelectedElement = node;
                    InDrawerViewMode = true;
                    break;
            }
        }

        private void OnNodeSelected(VertexSelectedEventArgs e)
        {
            if (e.MouseArgs.LeftButton != MouseButtonState.Pressed) return;

            switch (Mode)
            {
                case GraphMode.Select:
                    SelectedElement = e.VertexControl.GetDataVertex<Node>();
                    InDrawerViewMode = true;
                    break;
                case GraphMode.Edit:
                    var pos = e.MouseArgs.GetPosition((IInputElement)e.VertexControl.Parent);
                    if (_pendingLink != default)
                    {
                        _pendingLink.SetTarget(e.VertexControl);
                        // prevent self-loop
                        if (_pendingLink.Source.ID == _pendingLink.Target.ID) return;
                        PendingLinkCompleted?.Invoke(_pendingLink);

                        var link = AddLink(_pendingLink.Source, _pendingLink.Target, 100, LinkMode);
                        LinkRequested?.Invoke(link);
                        _pendingLink = null;

                        SelectedElement = link;
                        InDrawerViewMode = true;
                    }
                    else
                    {
                        _pendingLink = new PendingLink(e.VertexControl, pos.X, pos.Y, new SolidColorBrush(Colors.Blue));
                        PendingLinkRequested?.Invoke(_pendingLink);
                    }
                    break;
            }
        }

        private Node AddNode(string name, NodeType type)
        {
            var node = new Node(name, type);
            Nodes.Add(node);

            return node;
        }

        private Link AddLink(Node source, Node target, long weight, LinkType linkType, FiberType fiberType = FiberType.SSMF)
        {
            var link = Link.Create(source, target, weight, linkType, fiberType);
            Links.Add(link);

            return link;
        }
    }
}
