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

namespace Router.ViewModels
{
    public class GraphViewModel : BindableBase, IGraphViewModel
    {
        private PendingLink _pendingLink;
        public IGXLogicCore<Node, Link, Graph> LogicCore { get; private set; }
        #region [Commands and Events]
        public DelegateCommand Initialize { get; private set; }
        public DelegateCommand<object> SetSelectMode { get; private set; }
        public DelegateCommand<object> SetEditMode { get; private set; }
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

        private bool _inSelectionMode;
        public bool InSelectMode
        {
            get => _inSelectionMode;
            set => SetProperty(ref _inSelectionMode, value);
        }

        private bool _inEditMode;
        public bool InEditMode
        {
            get => _inEditMode;
            set => SetProperty(ref _inEditMode, value);
        }
        #endregion

        public GraphViewModel()
        {
            LogicCore = new GXLogicCore<Node, Link, Graph>(new Graph())
            {
                DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK,
                DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA,
                DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER,
            };
            LogicCore.DefaultOverlapRemovalAlgorithmParams = LogicCore.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);
            LogicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            LogicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            InSelectMode = true;
            Mode = GraphMode.Select;

            SetSelectMode = new DelegateCommand<object>((check) =>
            {
                if ((bool)check)
                {
                    InEditMode = false;
                    Mode = GraphMode.Select;
                }
                else
                {
                    InSelectMode = true;
                }
            });

            SetEditMode = new DelegateCommand<object>((check) =>
            {
                if ((bool)check)
                {
                    InSelectMode = false;
                    Mode = GraphMode.Edit;
                }
                else
                {
                    InEditMode = true;
                }
            });

            NodeSelected = new DelegateCommand<VertexSelectedEventArgs>(OnNodeSelected);
            CanvasInteraction = new DelegateCommand<MouseButtonEventArgs>(OnCanvasInteraction);
        }

        private void OnCanvasInteraction(MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            switch (Mode)
            {
                case GraphMode.None:
                    break;
                case GraphMode.Select:
                    break;
                case GraphMode.Edit:
                    var pos = e.GetPosition((UIElement)e.Source);
                    var node = AddNode($"#{Nodes.Count + 1} Node");

                    NodeRequested.Invoke(node, pos);
                    break;
            }
        }

        private void OnNodeSelected(VertexSelectedEventArgs a)
        {
            if (a.MouseArgs.LeftButton != MouseButtonState.Pressed) return;

            switch (Mode)
            {
                case GraphMode.None:
                    break;
                case GraphMode.Select:
                    break;
                case GraphMode.Edit:
                    var pos = a.MouseArgs.GetPosition((IInputElement)a.VertexControl.Parent);
                    var point = a.VertexControl.GetConnectionPointAt(pos);

                    if (point == null)
                    {
                        return;
                    }
                    else if (_pendingLink != default)
                    {
                        _pendingLink.SetTarget(a.VertexControl);
                        PendingLinkCompleted?.Invoke(_pendingLink);

                        var link = AddLink(_pendingLink.Source, _pendingLink.Target);
                        LinkRequested?.Invoke(link);
                        _pendingLink = null;
                    }
                    else
                    {
                        _pendingLink = new PendingLink(a.VertexControl, pos.X, pos.Y, new SolidColorBrush(Colors.Blue));
                        PendingLinkRequested?.Invoke(_pendingLink);
                    }
                    break;
            }
        }

        private Node AddNode(string name)
        {
            var node = new Node(name);
            Nodes.Add(node);

            return node;
        }

        private Link AddLink(Node source, Node target, long weight = 1)
        {
            var link = new Link(source, target, weight);
            Links.Add(link);

            return link;
        }
    }
}
