using GraphX.Controls;
using Router.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Router.Model
{
    public class PendingLink
    {
        public Node Source { get; set; }
        public NodeControl SourcePoint { get; set; }
        public Node Target { get; set; }
        public NodeControl TargetPoint { get; set; }
        public Point TargetPos { get; set; }
        public Path LinkPath { get; set; }

        public PendingLink(VertexControl sourcePoint, double x, double y, Brush brush)
        {
            LinkPath = new Path() { Stroke = brush, Data = new LineGeometry() };
            TargetPos = new Point(x, y);
            Source = sourcePoint.GetDataVertex<Node>();
            SourcePoint = (NodeControl)sourcePoint;
        }

        internal void UpdateTargetPosition(Point point)
        {
            TargetPos = point;
            UpdateGeometry(SourcePoint.GetCenterPosition(), point);
        }

        internal void SetTarget(VertexControl nodeControl)
        {
            TargetPoint = (NodeControl)nodeControl;
            Target = nodeControl?.GetDataVertex<Node>();
        }

        private void UpdateGeometry(Point start, Point end)
        {
            LinkPath.Data = new LineGeometry(start, end);
            (LinkPath.Data as LineGeometry).Freeze();
        }
    }
}
