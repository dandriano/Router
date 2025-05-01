using QuikGraph;
using System;

namespace Router.Interfaces
{
    public interface ILink : IEdge<IVertex>
    {
        Guid Id { get; }
    }
}