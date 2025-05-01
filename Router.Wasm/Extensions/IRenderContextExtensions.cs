using Blazorex;

namespace Router.Wasm.Extensions
{
    public static class IRenderContextExtensions 
    {
        public static void ClearFrame(this IRenderContext ctx, NetworkSettings settings) 
            => ctx.ClearRect(0, 0, settings.Width, settings.Height);

        public static void FillFrame(this IRenderContext ctx, NetworkSettings settings)
        {
            ctx.FillStyle = settings.BgColour;
            ctx.FillRect(0, 0, settings.Width, settings.Height);
        }

        public static void DrawLine(this IRenderContext ctx, float xs, float ys, float xe, float ye, string colour = "black", int width = 1)
        {
            ctx.BeginPath();
            ctx.MoveTo(xs, ys);
            ctx.LineTo(xe, ye);

            ctx.SetStroke(colour, width);
            ctx.Stroke();
        }

        public static void DrawGrid(this IRenderContext ctx, NetworkSettings settings) 
        {
            var centerX = settings.Width / 2;
            var centerY = settings.Height / 2;

            // redraw background
            ctx.FillFrame(settings);

            // draw vertical
            for (var x = 0; x <= settings.Width; x += settings.Spacing)
            {
                // main Y axis
                if (x == centerX)
                    ctx.DrawLine(x, 0, x, settings.Height, width: 3);
                // every 10th
                else if ((x - centerX) / settings.Spacing % 10 == 0)
                    ctx.DrawLine(x, 0, x, settings.Height, width: 2);
                else
                    ctx.DrawLine(x, 0, x, settings.Height, settings.FgColour);
            }

            // draw horizontal
            for (var y = 0; y <= settings.Height; y += settings.Spacing)
            {
                // main X axis
                if (y == centerY)
                    ctx.DrawLine(0, y, settings.Width, y, width: 3);
                // every 10th
                else if ((y - centerY) / settings.Spacing % 10 == 0)
                    ctx.DrawLine(0, y, settings.Width, y, width: 2);
                else
                    ctx.DrawLine(0, y, settings.Width, y, settings.FgColour);
            }
        }

        public static void SetStroke(this IRenderContext ctx, string colour = "black", int width = 1) 
        {
            ctx.StrokeStyle = colour;
            ctx.LineWidth = width;
        }
    }
}
