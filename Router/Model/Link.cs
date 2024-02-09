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
        public static Link Create(Node source, Node target, long weight, LinkType linkType, FiberType fiberType = FiberType.SSMF)
        {
            var forward = new Link(source, target, weight, fiberType);
            var backward = new Link(target, source, weight, fiberType);

            forward.SetPair(backward, linkType);
            backward.SetPair(forward, linkType);

            return forward;
        }

        public LinkType Type { get; private set; }
        public FiberType FiberType { get; set; }
        public Link BackwardLink { get; private set; }
        public Visibility SourcePointerVisibility => Type == LinkType.Duplex ? Visibility.Visible : Visibility.Collapsed;

        protected Link(Node source, Node target, long weight, FiberType fiberType = FiberType.SSMF) : base(source, target, weight)
        {
            Type = LinkType.None;
            FiberType = fiberType;
        }

        private void SetPair(Link backward, LinkType linkType)
        {
            if (Type == linkType) throw new Exception($"Already {linkType}");

            BackwardLink = backward;
            Type = linkType;
        }
    }
}
