using Router.Enums;
using Router.Interfaces;
using System;

namespace Router
{
    /// <summary>
    /// Representation of a network node on a graph
    /// </summary>
    public class Node : IVertex
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; private set; }
        public NodeType Type { get; private set; }
        public bool IsInRoute { get; private set; }
        public bool IsOutRoute { get; private set; }

        public Node(string name, NodeType type = NodeType.Terminal)
        {
            Name = name;
            Type = type;
        }
    }
}
