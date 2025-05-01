using Blazorex;
using Router.Wasm.Extensions;

namespace Router.Wasm
{
    /// <summary>
    /// A basic renderer for a bidirectional graph
    /// </summary>
    /// <remarks>
    /// Make generic someday? And also generic <see cref="NetworkSettings"/>.
    /// </remarks>
    public class NetworkRenderer
    {
        private readonly NetworkSettings _settings;

        public NetworkRenderer(NetworkSettings settings)
        {
            _settings = settings;
        }
        public void Render(IRenderContext ctx, Network network)
        {
            // TODO: to implement (as extensions to IRenderContext or as private methods right here..)
            ctx.DrawGrid(_settings);
            // ctx.DrawVerticies(network.Verticies)
            // ctx.DrawEdges(network.Edges)
        }
    }
}