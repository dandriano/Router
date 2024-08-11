using Router.Enums;
using Router.Interfaces;
using System;

namespace Router.Model
{
    /// <summary>
    /// Representation of a network node on a graph
    /// </summary>
    public class Node : IVertex
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public NodeType Type { get; set; }
        public bool IsInRoute { get; set; }
        public bool IsOutRoute { get; set; }

        public Node(Guid id, string name, NodeType type = NodeType.Terminal)
        {
            Id = id;
            Name = name;
            Type = type;
        }
    }
}
