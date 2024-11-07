using GLGraphs.CartesianGraph;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Wpf;
using Router.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Router.Controls
{
    public class GraphAreaControl : UserControl
    {
        /// <summary>
        /// Event fired before the graph is updated & rendered.
        /// </summary>
        public event Action<TimeSpan> Render;
        public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(nameof(Settings), typeof(CartesianGraphSettings), typeof(GraphAreaControl), new PropertyMetadata(default(CartesianGraphSettings)));
        public CartesianGraphSettings Settings
        {
            get => (CartesianGraphSettings)GetValue(SettingsProperty);
            set => SetValue(SettingsProperty, value);
        }
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof(State), typeof(CartesianGraphState<IVertex>), typeof(GraphAreaControl), new PropertyMetadata(default(CartesianGraphState<IVertex>)));
        public CartesianGraphState<IVertex> State
        {
            get => (CartesianGraphState<IVertex>)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }
        public static readonly DependencyProperty VertexRequestedCommand = DependencyProperty.Register(nameof(VertexRequested), typeof(ICommand), typeof(GraphAreaControl), new PropertyMetadata(default(ICommand)));
        public ICommand VertexRequested
        {
            get => (ICommand)GetValue(VertexRequestedCommand);
            set => SetValue(VertexRequestedCommand, value);
        }
        #region [GL]
        private GLWpfControl _control;
        private CartesianGraphRenderer<IVertex> _renderer;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var settings = new GLWpfControlSettings();
            _control = new GLWpfControl();
            _control.Ready += OnReady;
            Content = _control;
            _control.Start(settings);
        }
        private void OnReady()
        {
            _renderer = new CartesianGraphRenderer<IVertex>(Settings);
            _control.Render += OnRender;
        }

        public void ResetView()
        {
            if (State == null)
                return;

            State.Camera.Target.Position = Vector2.Zero;
            State.Camera.Target.VerticalSize = 1f;
            State.IsCameraAutoControlled = true;
        }

        private Vector2 ClientToView(Point pt)
        {
            var result = new Vector2((float)pt.X, (float)pt.Y);

            result.X /= (float)_control.RenderSize.Width;
            result.Y /= (float)_control.RenderSize.Height;

            return result;
        }

        private void OnRender(TimeSpan deltaTime)
        {
            if (_renderer == null || State == null)
                return;

            Render?.Invoke(deltaTime);
            var renderSize = _control.RenderSize;
            State.ViewportHeight = (float)renderSize.Height;
            State.ViewportHeight = (float)renderSize.Width;

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, (int)renderSize.Width, (int)renderSize.Height);

            float aspectRatio = (float)(renderSize.Width / renderSize.Height);
            State.Camera.Target.AspectRatio = aspectRatio;
            State.Camera.Current.AspectRatio = aspectRatio;
            State.Update((float)deltaTime.TotalSeconds);
            _renderer.Render(State);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var pos = ClientToView(e.GetPosition(_control));

            State.MousePosition = pos;
            if (State.TryGetMouseover(pos, out var targetPt))
            {
                State.MouseoverTarget = targetPt;
            }
            else
            {
                State.MouseoverTarget = null;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (State == null)
                return;

            var num = (float)e.Delta / 120f;
            State.Camera.Target.ZoomIn(num * 10f);
            State.IsCameraAutoControlled = false;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.ChangedButton == MouseButton.Middle)
                ResetView();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (VertexRequested.CanExecute(this))
                VertexRequested.Execute(this);
        }
        #endregion
    }
}
