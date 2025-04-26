using QuikGraph;
using Router.Interfaces;

namespace Router
{
    /// <summary>
    /// Network as bidirectional graph
    /// </summary>
    public class Network : BidirectionalGraph<IVertex, Link>
    {
        public override bool AddVertex(IVertex vertex)
        {
            /*
            // rescale into view co-ordinates
            var (x, y) = State.MousePosition * 2.0f - Vector2.One;

            // project mouse into world
            var vpMat = State.Camera.Current.ViewProjection;
            vpMat = Matrix4.CreateScale(1.0f, State.YScale, 1.0f) * vpMat;
            vpMat.Invert();

            var worldPos = (new Vector4(x, -y, 0, 1) * vpMat).Xy;
            State.Series[0].Add(vertex, worldPos.X, worldPos.Y);
            */
            return base.AddVertex(vertex);
        }
    }
}
