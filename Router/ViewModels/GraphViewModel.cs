using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Router.Enums;
using Router.Model;
using System;

namespace Router.ViewModels
{
    public class GraphViewModel : ObservableObject
    {
        #region [Fields and Properties]
        public Network Network { get; private set; }
        #region [Commands and Events]
        public RelayCommand AddVertex { get; private set; }
        /*
        public event Action<GraphMode> GraphModeChanged;
        public event Action<Node, Point> NodeRequested;
        public event Action<Link> LinkRequested;
        */
        #endregion
        #region [Observables]
        private int _vertexCount;
        public int VertexCount
        {
            get => _vertexCount;
            set => SetProperty(ref _vertexCount, value);
        }
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
            set => SetProperty(ref _mode, value);
            /*
             * () =>
                {
                    if (value != GraphMode.Edit && _pendingLink != default)
                    {
                        PendingLinkCompleted?.Invoke(_pendingLink);
                        _pendingLink = null;
                    }
                    GraphModeChanged?.Invoke(value);
                }
             */
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
            Network = new Network();
            Mode = GraphMode.Select;
            NodeMode = NodeType.Terminal;
            LinkMode = LinkType.Simplex;

            Network.VertexAdded += (n) =>
            {
                VertexCount = Network.VertexCount;
            };

            AddVertex = new RelayCommand(() =>
            {
                InDrawerViewMode = true;
                SelectedElement = AddNode($"#{Network.VertexCount + 1} Node", NodeMode);
            }, () =>
            {
                return Mode == GraphMode.Edit;
            });
        }

        private Node AddNode(string name, NodeType type)
        {
            var n = new Node(Guid.NewGuid(), name, type);
            if (!Network.AddVertex(n))
                throw new InvalidOperationException();
            return n;
        }

        private Link AddLink(Node source, Node target, long weight, LinkType linkType, FiberType fiberType = FiberType.SSMF)
        {
            var l = Link.Create(source, target, weight, linkType, fiberType);
            if (!Network.AddEdge(l))
                throw new InvalidOperationException();

            return l;
        }
    }
}
