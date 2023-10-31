using GraphX.Controls;
using GraphX.Controls.Models;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Router.Model
{
    public class PendingLink : IDisposable
    {
        public VertexControl Source { get; set; }
        public IVertexConnectionPoint SourcePoint { get; set; }
        public Point TargetPos { get; set; }
        public Path LinkPath { get; set; }

        public PendingLink(VertexControl source, IVertexConnectionPoint sourcePoint, Point targetPos, Brush brush)
        {
            LinkPath = new Path() { Stroke = brush, Data = new LineGeometry() };
            TargetPos = targetPos;
            Source = source;
            SourcePoint = sourcePoint;
            Source.PositionChanged += Source_PositionChanged;
        }

        void Source_PositionChanged(object sender, VertexPositionEventArgs args)
        {
            UpdateGeometry(Source.GetCenterPosition(), TargetPos);
        }

        internal void UpdateTargetPosition(Point point)
        {
            TargetPos = point;
            UpdateGeometry(Source.GetCenterPosition(), point);
        }

        private void UpdateGeometry(Point start, Point end)
        {
            LinkPath.Data = new LineGeometry(start, end);
            (LinkPath.Data as LineGeometry).Freeze();
        }

        public void Dispose()
        {
            Source.PositionChanged -= Source_PositionChanged;
            Source = null;
            GC.SuppressFinalize(this);
        }
    }
}
