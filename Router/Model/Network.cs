using GLGraphs.CartesianGraph;
using OpenTK.Mathematics;
using QuikGraph;
using Router.Interfaces;

namespace Router.Model
{
    /// <summary>
    /// Network as bidirectional graph
    /// </summary>
    public class Network : BidirectionalGraph<Node, Link>
    {
        public CartesianGraphSettings Settings { get; } = CartesianGraphSettings.Default;
        public CartesianGraphState<IVertex> State { get; }
       
        public Network()
        {
            Settings.BackgroundColor = new Color4(150, 150, 150, 0);
            
            State = new CartesianGraphState<IVertex>(Settings);

            State.XGridSpacing.Automatic = false;
            State.XGridSpacing.Major = 1;
            State.XGridSpacing.Minor = 0.5f;

            State.YGridSpacing.Automatic = false;
            State.YGridSpacing.Major = 1;
            State.YGridSpacing.Minor = 0.5f;

            State.AddSeries(SeriesType.Point, "VERTICES");
            State.AddSeries(SeriesType.Line, "EDGES");
        }

        public override bool AddVertex(Node vertex)
        {
            // rescale into view co-ordinates
            var (x, y) = State.MousePosition * 2.0f - Vector2.One;

            // project mouse into world
            var vpMat = State.Camera.Current.ViewProjection;
            vpMat = Matrix4.CreateScale(1.0f, State.YScale, 1.0f) * vpMat;
            vpMat.Invert();

            var worldPos = (new Vector4(x, -y, 0, 1) * vpMat).Xy;
            State.Series[0].Add(vertex, worldPos.X, worldPos.Y);

            return base.AddVertex(vertex);
        }
    }
}
