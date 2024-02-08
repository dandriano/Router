using GraphX.Common.Models;
using Router.Enums;

namespace Router.Model
{
    /// <summary>
    /// Representation of a network node on a graph
    /// </summary>
    public class Node : VertexBase
    {
        public string Name { get; set; }
        public NodeType Type { get; set; }
        public bool IsInRoute { get; set; }
        public bool IsOutRoute { get; set; }

        public Node(string name, NodeType type = NodeType.Terminal) : base()
        {
            Name = name;
            Type = type;
        }

        public Node(long id, string name, NodeType type = NodeType.Terminal)
        {
            ID = id;
            Name = name;
            Type = type;
        }
    }
}
