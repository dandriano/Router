using GraphX.Common.Models;

namespace Router.Model
{
    /// <summary>
    /// Physical link as a fiber span between network nodes/circuit-packs
    /// </summary>
    public class Link : EdgeBase<Node>
    {
        public Link(Node source, Node target, long weight = 1) : base(source, target, weight)
        {
            // SourceConnectionPointId = 1;
            // TargetConnectionPointId = 1;
        }
    }
}
