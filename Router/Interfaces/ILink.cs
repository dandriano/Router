using System;
using QuikGraph;

namespace Router.Interfaces
{
    public interface ILink : IEdge<IVertex>
    {
        Guid Id { get; }
    }
}