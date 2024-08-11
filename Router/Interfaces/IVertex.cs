using System;

namespace Router.Interfaces
{
    public interface IVertex
    {
        Guid Id { get; }
        string Name { get; set; }
    }
}