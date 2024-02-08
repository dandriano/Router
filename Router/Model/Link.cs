using GraphX.Common.Models;
using Router.Enums;
using System;
using System.Windows;

namespace Router.Model
{
    /// <summary>
    /// Physical link as a fiber span between network nodes/circuit-packs
    /// </summary>
    public class Link : EdgeBase<Node>
    {
        public LinkType Type { get; private set; }
        public FiberType FiberType { get; private set; }
        protected Link BackwardLink { get; private set; }
        public Visibility SourcePointerVisibility => Type == LinkType.Duplex ? Visibility.Visible : Visibility.Collapsed;

        public Link(Node source, Node target, long weight = 1, LinkType linkType = LinkType.Simplex, FiberType fiberType = FiberType.SSMF) : base(source, target, weight)
        {
            Type = linkType;
            FiberType = fiberType;
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
