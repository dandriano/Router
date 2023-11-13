using GraphX.Common.Models;

namespace Router.Model
{
    public class Link : EdgeBase<Node>
    {
        public Link(Node source, Node target, long weight = 1) : base(source, target, weight)
        {
            // SourceConnectionPointId = 1;
            // TargetConnectionPointId = 1;
        }
    }
}
