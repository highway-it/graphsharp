﻿namespace GraphSharp.Algorithms.Layout.Simple.Hierarchical
{
    public class EfficientSugiyamaLayoutParameters : LayoutParametersBase
    {
        private LayoutDirection _direction = LayoutDirection.TopToBottom;
        private double _layerDistance = 15.0;
        private double _vertexDistance = 15.0;
        private int _positionMode = -1;
        private bool _optimizeIsolatedVertices = false;
        private bool _optimizeWidth = false;
        private double _widthPerHeight = 1.0;
        private bool _minimizeEdgeLength = true;
        private bool _ignoreCollapsedVertices = false;
        internal const int MaxPermutations = 50;
        private SugiyamaEdgeRoutings _edgeRouting = SugiyamaEdgeRoutings.Traditional;

        public LayoutDirection Direction
        {
            get { return _direction; }
            set
            {
                if (value == _direction)
                    return;

                _direction = value;
                NotifyPropertyChanged("Direction");
            }
        }

        public double LayerDistance
        {
            get { return _layerDistance; }
            set
            {
                if (value == _layerDistance)
                    return;

                _layerDistance = value;
                NotifyPropertyChanged("LayerDistance");
            }
        }

        public double VertexDistance
        {
            get { return _vertexDistance; }
            set
            {
                if (value == _vertexDistance)
                    return;

                _vertexDistance = value;
                NotifyPropertyChanged("VertexDistance");
            }
        }

        public int PositionMode
        {
            get { return _positionMode; }
            set
            {
                if (value == _positionMode)
                    return;

                _positionMode = value;
                NotifyPropertyChanged("PositionMode");
            }
        }

        public double WidthPerHeight
        {
            get { return _widthPerHeight; }
            set
            {
                if (value == _widthPerHeight)
                    return;

                _widthPerHeight = value;
                NotifyPropertyChanged("WidthPerHeight");
            }
        }

        public bool OptimizeIsolatedVertices
        {
            get { return _optimizeIsolatedVertices; }
            set
            {
                if (value == _optimizeIsolatedVertices)
                    return;

                _optimizeIsolatedVertices = value;
                NotifyPropertyChanged("OptimizeIsolatedVertices");
            }
        }

        public bool OptimizeWidth
        {
            get { return _optimizeWidth; }
            set
            {
                if (value == _optimizeWidth)
                    return;

                _optimizeWidth = value;
                NotifyPropertyChanged("OptimizeWidth");
            }
        }

        public bool MinimizeEdgeLength
        {
            get { return _minimizeEdgeLength; }
            set
            {
                if (value == _minimizeEdgeLength)
                    return;

                _minimizeEdgeLength = value;
                NotifyPropertyChanged("MinimizeEdgeLength");
            }
        }

        public bool IgnoreCollapsedVertices
        {
            get { return _ignoreCollapsedVertices; }
            set
            {
                if (value == _ignoreCollapsedVertices)
                    return;

                _ignoreCollapsedVertices = value;
                NotifyPropertyChanged("IgnoreCollapsedVertices");
            }
        }

        public SugiyamaEdgeRoutings EdgeRouting
        {
            get { return _edgeRouting; }
            set
            {
                if (value == _edgeRouting)
                    return;

                _edgeRouting = value;
                NotifyPropertyChanged("EdgeRouting");
            }
        }
	}
}