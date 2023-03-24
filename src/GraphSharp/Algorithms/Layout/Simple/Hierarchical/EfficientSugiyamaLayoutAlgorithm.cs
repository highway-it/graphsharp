using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using GraphSharp.Algorithms.EdgeRouting;

using QuickGraph;

namespace GraphSharp.Algorithms.Layout.Simple.Hierarchical
{
    public partial class EfficientSugiyamaLayoutAlgorithm<TVertex, TEdge, TGraph> 
        : DefaultParameterizedLayoutAlgorithmBase<TVertex, TEdge, TGraph, EfficientSugiyamaLayoutParameters>,
          IEdgeRoutingAlgorithm<TVertex, TEdge, TGraph>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
    {
        /// <summary>
        /// The copy of the VisitedGraph which should be laid out.
        /// </summary>
        private IMutableBidirectionalGraph<SugiVertex, SugiEdge> _graph;

        /// <summary>
        /// Routing points for the edges of the original graph.
        /// </summary>
        private readonly IDictionary<TEdge, Point[]> _edgeRoutingPoints =
            new Dictionary<TEdge, Point[]>();

        private readonly IDictionary<TEdge, IList<SugiVertex>> _dummyVerticesOfEdges =
            new Dictionary<TEdge, IList<SugiVertex>>();

        private readonly IDictionary<TVertex, Size> _vertexSizes;

        private readonly IDictionary<TVertex, SugiVertex> _vertexMap = 
            new Dictionary<TVertex, SugiVertex>();

        /// <summary>
        /// Isolated vertices in the visited graph, which will be handled only in
        /// the last step of the layout.
        /// </summary>
        private List<SugiVertex> _isolatedVertices;

        /// <summary>
        /// Edges that has been involved in cycles in the original graph. (These has
        /// been reverted during this layout algorithm).
        /// </summary>
        private readonly IList<TEdge> _cycleEdges = new List<TEdge>();

        /// <summary>
        /// It stores the vertices or segments which inside the layers.
        /// </summary>
        private readonly IList<IList<SugiVertex>> _layers =
            new List<IList<SugiVertex>>();

        public EfficientSugiyamaLayoutAlgorithm(
            TGraph visitedGraph, 
            EfficientSugiyamaLayoutParameters parameters, 
            IDictionary<TVertex, Size> vertexSizes)
            : this(visitedGraph, parameters, null, vertexSizes)
        { }

        public EfficientSugiyamaLayoutAlgorithm(
            TGraph visitedGraph, 
            EfficientSugiyamaLayoutParameters parameters, 
            IDictionary<TVertex, Point> vertexPositions,
            IDictionary<TVertex, Size> vertexSizes)
            : base(visitedGraph, vertexPositions, parameters)
        {
            _vertexSizes = vertexSizes;
        }

        /// <summary>
        /// Initializes the private _graph field which stores the graph that 
        /// we operate on.
        /// </summary>
        private void InitTheGraph()
        {
            //make a copy of the original graph
            _graph = new BidirectionalGraph<SugiVertex, SugiEdge>();

            var ignoreCollapsedVertices = Parameters.IgnoreCollapsedVertices;

            //copy the vertices
            foreach (var vertex in VisitedGraph.Vertices)
            {
                Size size = new Size();
                if (_vertexSizes != null)
                    _vertexSizes.TryGetValue(vertex, out size);

                if (ignoreCollapsedVertices && (size.Width == 0.0 || size.Height == 0.0))
                {
                    VertexPositions[vertex] = default;
                    continue;
                }

                var vertexWrapper = new SugiVertex(vertex, size);
                _graph.AddVertex(vertexWrapper);
                _vertexMap[vertex] = vertexWrapper;
            }

            //copy the edges
            foreach (var edge in VisitedGraph.Edges)
            {
                if (_vertexMap.TryGetValue(edge.Source, out var source) && _vertexMap.TryGetValue(edge.Target, out var target))
                    _graph.AddEdge(new SugiEdge(edge, source, target));
            }
        }

        protected override void InternalCompute()
        {
            InitTheGraph();

            var angle = Parameters.Direction switch
            {
                LayoutDirection.LeftToRight => -90.0,
                LayoutDirection.TopToBottom =>   0.0,
                LayoutDirection.RightToLeft =>  90.0,
                LayoutDirection.BottomToTop => 180.0,
                _                           =>   0.0
            };

            if (angle == -90.0 || angle == 90.0)
            {
                foreach (var vertex in _graph.Vertices)
                    vertex.Size = new Size(vertex.Size.Height, vertex.Size.Width);
            }

            //first step
            DoPreparing();

            BuildSparseNormalizedGraph();
            DoCrossingMinimizations();
            CalculatePositions();

            if (angle != 0.0)
            {
                var transform = new RotateTransform(angle);

                foreach (var (vextex, position) in VertexPositions.ToList())
                    VertexPositions[vextex] = transform.Transform(position);

                foreach (var route in EdgeRoutes.Values.ToList())
                    for (var index = 0; index < route.Length; index++)
                        route[index] = transform.Transform(route[index]);
            }
        }

        #region IEdgeRoutingAlgorithm<TVertex,TEdge,TGraph> Members

        public IDictionary<TEdge, Point[]> EdgeRoutes
        {
            get { return _edgeRoutingPoints; }
        }

        #endregion
    }
}