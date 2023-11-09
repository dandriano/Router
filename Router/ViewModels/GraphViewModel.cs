using Prism.Commands;
using Prism.Mvvm;
using Router.Enums;
using Router.Interfaces;
using Router.Model;
using System.Collections.ObjectModel;

namespace Router.ViewModels
{
    public class GraphViewModel : BindableBase
    {
        private bool _inSelectionMode;
        private bool _inEditMode;

        public IGraphController GraphController { get; private set; }
        public ObservableCollection<Node> Nodes { get; private set; } = new ObservableCollection<Node>();
        public ObservableCollection<Link> Links { get; private set; } = new ObservableCollection<Link>();
        public DelegateCommand Initialize { get; private set; }
        public DelegateCommand<object> SetSelectMode { get; private set; }
        public DelegateCommand<object> SetEditMode { get; private set; }

        public bool InSelectMode
        {
            get => _inSelectionMode;
            set => SetProperty(ref _inSelectionMode, value);
        }

        public bool InEditMode
        {
            get => _inEditMode;
            set => SetProperty(ref _inEditMode, value);
        }

        public GraphViewModel(IGraphController graphController)
        {
            GraphController = graphController;
            GraphController.NodeRequested += GraphNodeAdded;
            //GraphController.Graph.VertexRemoved += GraphNodeRemoved;
            GraphController.LinkRequested += GraphLinkAdded;
            //GraphController.Graph.EdgeRemoved += GraphLinkRemoved;
            InSelectMode = true;

            SetSelectMode = new DelegateCommand<object>((check) =>
            {
                if ((bool)check)
                {
                    InEditMode = false;
                    GraphController.SetGraphMode(GraphMode.Select);
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
                    GraphController.SetGraphMode(GraphMode.Edit);
                }
                else
                {
                    InEditMode = true;
                }
            });
        }

        private void GraphLinkRemoved(Link link)
        {
            Links.Remove(link);
        }

        private void GraphLinkAdded(Link link)
        {
            Links.Add(link);
        }

        private void GraphNodeRemoved(Node node)
        {
            Nodes.Remove(node);
        }

        private void GraphNodeAdded(Node node, double x, double y)
        {
            Nodes.Add(node);
        }
    }
}
