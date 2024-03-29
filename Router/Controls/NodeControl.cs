﻿using GraphX.Controls;
using Router.Enums;
using System.Windows;
using System.Windows.Media;

namespace Router.Controls
{
    public class NodeControl : VertexControl
    {
        public static readonly DependencyProperty NodeTypeProperty =
                DependencyProperty.Register("NodeType", typeof(NodeType), typeof(NodeControl), new UIPropertyMetadata(null));

        public NodeType NodeType
        {
            get => (NodeType)GetValue(NodeTypeProperty);
            set => SetValue(NodeTypeProperty, value);
        }

        public static readonly DependencyProperty NodeTemplateProperty =
                DependencyProperty.Register("NodeTemplate", typeof(DataTemplate), typeof(NodeControl), new UIPropertyMetadata(null));

        public DataTemplate NodeTemplate
        {
            get => (DataTemplate)GetValue(NodeTemplateProperty);
            set => SetValue(NodeTemplateProperty, value);
        }

        public static readonly DependencyProperty NodeBrushProperty =
                DependencyProperty.Register("NodeBrush", typeof(Brush), typeof(NodeControl), new UIPropertyMetadata(null));

        public Brush NodeBrush
        {
            get => (Brush)GetValue(NodeBrushProperty);
            set => SetValue(NodeBrushProperty, value);
        }

        public static readonly DependencyProperty NodeWarnBrushProperty =
                DependencyProperty.Register("NodeWarnBrush", typeof(Brush), typeof(NodeControl), new UIPropertyMetadata(null));

        public Brush NodeWarnBrush
        {
            get => (Brush)GetValue(NodeWarnBrushProperty);
            set => SetValue(NodeWarnBrushProperty, value);
        }

        public static readonly DependencyProperty NodeWarnBackgroundBrushProperty =
                DependencyProperty.Register("NodeWarnBackgroundBrush", typeof(Brush), typeof(NodeControl), new UIPropertyMetadata(null));

        public Brush NodeWarnBackgroundBrush
        {
            get => (Brush)GetValue(NodeWarnBackgroundBrushProperty);
            set => SetValue(NodeWarnBackgroundBrushProperty, value);
        }

        public NodeControl(object vertexData, bool tracePositionChange = true, bool bindToDataObject = true) : base(vertexData, tracePositionChange, bindToDataObject)
        {
        }
    }
}
