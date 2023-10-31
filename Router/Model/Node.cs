using GraphX.Common.Models;

namespace Router.Model
{
    public class Node : VertexBase
    {
        public string Name { get; set; }
        public bool IsInRoute { get; set; }
        public bool IsOutRoute { get; set; }

        public Node(long id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
