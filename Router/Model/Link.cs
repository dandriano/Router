using GraphX.Common.Models;
using Router.Enums;
using System;

namespace Router.Model
{
    public class Link : EdgeBase<Node>
    {
        public LinkType Type { get; private set; }
        protected Link BackwardLink { get; private set; }

        public Link(Node source, Node target, long weight = 1) : base(source, target, weight)
        {
            Type = LinkType.Simplex;
            // SourceConnectionPointId = 1;
            // TargetConnectionPointId = 1;
        }

        public void SetDuplex(Link backward)
        {
            if (Type != LinkType.Simplex)
            {
                throw new Exception("Already duplex");
            }

            BackwardLink = backward;
            Type = LinkType.Duplex;
        }

        public void SetSimplex() 
        {
            if (Type != LinkType.Duplex)
            {
                throw new Exception("Already simplex");
            }

            BackwardLink = null;
            Type = LinkType.Simplex;
        }
    }
}
