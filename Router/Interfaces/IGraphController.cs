using Router.Model;

namespace Router.Interfaces
{
    public interface IGraphController
    {
        Graph Graph { get; }
        Link AddLink(Node source, Node target, long weight = 1);
        Node AddNode(long id, string name, double x, double y);
        void Initialize();
    }
}
