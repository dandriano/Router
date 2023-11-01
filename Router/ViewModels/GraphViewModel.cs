using Prism.Commands;
using Prism.Mvvm;
using Router.Interfaces;
using Router.Model;
using System.Collections.ObjectModel;

namespace Router.ViewModels
{
    public class GraphViewModel : BindableBase
    {
        public IGraphController GraphController { get; }
        public DelegateCommand InitializeController { get; }
        public ObservableCollection<Node> Nodes { get; private set; } = new ObservableCollection<Node>();
        public ObservableCollection<Link> Links { get; private set; } = new ObservableCollection<Link>();
        public GraphViewModel(IGraphController graphController)
        {
            GraphController = graphController;
            InitializeController = new DelegateCommand(GraphController.Initialize);

            GraphController.Graph.VertexAdded += GraphNodeAdded;
            GraphController.Graph.VertexRemoved += GraphNodeRemoved;
            GraphController.Graph.EdgeAdded += GraphLinkAdded;
            GraphController.Graph.EdgeRemoved += GraphLinkRemoved;
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

        private void GraphNodeAdded(Node node)
        {
            Nodes.Add(node);
        }
    }
}
