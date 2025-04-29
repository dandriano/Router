using Blazorex;

namespace Router.Wasm
{
    /// <summary>
    /// A basic renderer for a bidirectional graph (make generic someday?)
    /// </summary>
    public class NetworkRenderer
    {
        private NetworkSettings _settings;

        public NetworkRenderer(NetworkSettings settings)
        {
            _settings = settings;
        }

        public void Render(IRenderContext ctx, Network network)
        {
            // TODO: to implement (as extensions to IRenderContext or as private methods right here..)
            // ctx.DrawGrid(_settings);
            // ctx.DrawVerticies(network.Verticies)
            // ctx.DrawEdges(network.Edges)
        }
    }
}