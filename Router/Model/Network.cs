using QuikGraph;
using QuikGraph.Algorithms.ShortestPath;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Router.Model
{
    /// <summary>
    /// Network as bidirectional graph
    /// </summary>
    public class Network : BidirectionalGraph<Node, Link>
    {
        public IEnumerable<IEnumerable<Link>> ShortestPathsYen(Node root, Node target, int k,
            Func<Link, double> edgeWeights = null)
        {
            edgeWeights ??= (Link e) => { return e.Weight; };
            var g = Edges
                .Select(e => (ee: new EquatableTaggedEdge<Node, double>(e.Source, e.Target, edgeWeights(e)), e))
                .ToList();

            var algo = new YenShortestPathsAlgorithm<Node>(g.Select(v => v.ee)
                                                            .ToAdjacencyGraph<Node, EquatableTaggedEdge<Node, double>>(),
                                                           root,
                                                           target,
                                                           k);

            var result = new List<List<Link>>();
            foreach (var p in algo.Execute())
            {
                var innerResult = new List<Link>();
                foreach (var e in p)
                {
                    var i = g.FindIndex(t => t.ee == e);
                    innerResult.Add(g[i].e);
                    g.RemoveAt(i);
                }
                result.Add(innerResult);
            }
            return result;
        }
    }
}
