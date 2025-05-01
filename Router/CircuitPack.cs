using Router.Interfaces;
using System;

namespace Router
{
    /// <summary>
    /// Representation of a equipment board on a node
    /// </summary>
    public class CircuitPack : IVertex
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; private set; }

        public CircuitPack(string name)
        {
            Name = name;
        }
    }
}
