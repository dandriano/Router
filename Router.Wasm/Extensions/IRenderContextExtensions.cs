using Blazorex;
using System;

namespace Router.Wasm.Extensions
{
    public static class IRenderContextExtensions 
    {
        /// <summary>
        /// Clear entire frame.
        /// </summary>
        /// <param name="ctx">The rendering context.</param>
        /// <param name="settings"></param>
        public static void ClearFrame(this IRenderContext ctx, NetworkSettings settings) 
            => ctx.ClearRect(0, 0, settings.Width, settings.Height);
        /// <summary>
        /// Fill entire frame.
        /// </summary>
        /// <param name="ctx">The rendering context.</param>
        /// <param name="settings"></param>
        public static void FillFrame(this IRenderContext ctx, NetworkSettings settings)
        {
            ctx.FillStyle = settings.BgColour;
            ctx.FillRect(0, 0, settings.Width, settings.Height);
        }
        /// <summary>
        /// Draws a line on the rendering context from a starting point to an ending point.
        /// </summary>
        /// <param name="ctx">The rendering context to draw on.</param>
        /// <param name="xs">The X coordinate of the starting point of the line.</param>
        /// <param name="ys">The Y coordinate of the starting point of the line.</param>
        /// <param name="xe">The X coordinate of the ending point of the line.</param>
        /// <param name="ye">The Y coordinate of the ending point of the line.</param>
        /// <param name="colour">The color of the line (default is black).</param>
        /// <param name="width">The width (thickness) of the line (default is 1).</param>
        public static void StrokeLine(this IRenderContext ctx, float xs, float ys, float xe, float ye, string colour = "black", int width = 1)
        {
            ctx.BeginPath();
            ctx.MoveTo(xs, ys);
            ctx.LineTo(xe, ye);

            ctx.SetStroke(colour, width);
            ctx.Stroke();
        }
        /// <summary>
        /// Draw "cartesian" grid on canvas based on network settings
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="settings"></param>
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
                    ctx.StrokeLine(x, 0, x, settings.Height, width: 3);
                // every 10th
                else if ((x - centerX) / settings.Spacing % 10 == 0)
                    ctx.StrokeLine(x, 0, x, settings.Height, width: 2);
                else
                    ctx.StrokeLine(x, 0, x, settings.Height, settings.FgColour);
            }

            // draw horizontal
            for (var y = 0; y <= settings.Height; y += settings.Spacing)
            {
                // main X axis
                if (y == centerY)
                    ctx.StrokeLine(0, y, settings.Width, y, width: 3);
                // every 10th
                else if ((y - centerY) / settings.Spacing % 10 == 0)
                    ctx.StrokeLine(0, y, settings.Width, y, width: 2);
                else
                    ctx.StrokeLine(0, y, settings.Width, y, settings.FgColour);
            }
        }
        /// <summary>
        /// Strokes an ellipse on the given rendering context by approximating it with line segments.
        /// </summary>
        /// <param name="ctx">The rendering context to draw on.</param>
        /// <param name="cx">The X coordinate of the ellipse center.</param>
        /// <param name="cy">The Y coordinate of the ellipse center.</param>
        /// <param name="rx">The radius of the ellipse along the X axis.</param>
        /// <param name="ry">The radius of the ellipse along the Y axis.</param>
        /// <param name="colour">Colour stroke.</param>
        /// <param name="segments">The number of line segments used to approximate the ellipse. Higher means smoother.</param>
        public static void StrokeEllipse(this IRenderContext ctx, float cx, float cy, float rx, float ry, string colour = "black", int segments = 100)
        {
            ctx.BeginPath();

            // loop through 'segments' points around the ellipse perimeter
            for (var i = 0; i <= segments; i++)
            {
                // calculate angle for the current segment and determine coords
                var theta = (double)i / segments * 2.0 * Math.PI;
                var x = cx + rx * (float)Math.Cos(theta);
                var y = cy + ry * (float)Math.Sin(theta);

                if (i == 0)
                    ctx.MoveTo(x, y);
                else
                    ctx.LineTo(x, y);
            }

            ctx.SetStroke(colour);
            ctx.Stroke();
        }
        /// <summary>
        /// Fills an ellipse on the given rendering context.
        /// </summary>
        /// <param name="ctx">The rendering context to draw on.</param>
        /// <param name="cx">The X coordinate of the ellipse center.</param>
        /// <param name="cy">The Y coordinate of the ellipse center.</param>
        /// <param name="rx">The radius of the ellipse along the X axis.</param>
        /// <param name="ry">The radius of the ellipse along the Y axis.</param>
        /// <param name="colour">Colour stroke.</param>
        public static void FillEllipse(this IRenderContext ctx, float cx, float cy, float rx, float ry, string colour = "black")
        {
            // bounding rectangle
            var left = cx - rx;
            var top = cy - ry;
            var width = 2 * rx;
            var height = 2 * ry;

            // iterate through each point within the bounding rectangle
            for (var x = left; x < left + width; x++)
            {
                for (var y = top; y < top + height; y++)
                {
                    // check if the point (x, y) is inside the ellipse
                    var normalizedX = (x - cx) / rx;
                    var normalizedY = (y - cy) / ry;

                    if ((normalizedX * normalizedX + normalizedY * normalizedY) <= 1)
                    {
                        // fill a 1x1 rectangle at (x, y) if inside the ellipse
                        ctx.FillStyle = colour;
                        ctx.FillRect(x, y, 1, 1);
                    }
                }
            }
        }
        /// <summary>
        /// Shortcut for setting stroke style
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="colour"></param>
        /// <param name="width"></param>
        public static void SetStroke(this IRenderContext ctx, string colour = "black", int width = 1) 
        {
            ctx.StrokeStyle = colour;
            ctx.LineWidth = width;
        }
        /// <summary>
        /// Draw network verticies (if needed)
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="network"></param>
        public static void DrawVertices(this IRenderContext ctx, Network network)
        {
            foreach (var v in network.Vertices)
            {
                if (!network.VerticesMap.TryGetValue(v.Id, out var coords))
                    continue;

                // network.VerticesMap.Remove(v.Id);

                ctx.FillEllipse(coords.x, coords.y, 5, 5, "red");
                ctx.StrokeEllipse(coords.x, coords.y, 5, 5, "green");
            }
        }
    }
}
