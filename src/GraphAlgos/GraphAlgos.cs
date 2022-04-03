namespace GraphAlgos
{
	/// <summary>
	/// Represents a multi-pass Graph algorithm
	/// </summary>
	public abstract class GraphAlgos
	{
		/// <summary>Describes the state/color of the vertex</summary>
		public readonly byte[] VertexState;
		/// <summary>The graph on which the algorithm is working</summary>
		public Graph WorkingGraph;
		/// <summary>
		/// Base constructor for GraphAlgos
		/// </summary>
		/// <param name="g">The graph being worked on</param>
		public GraphAlgos(Graph g)
		{
			WorkingGraph = g;
			VertexState = new byte[g.VerticesCount];
		}
		/// <summary>Dimensions of the string table</summary>
		public abstract (byte Width, byte Height) TableDimensions { get; }
		/// <summary>The pass number of the algorithm</summary>
		public byte CurrentPass { get; protected set; }
		/// <summary>Describes if the algorithm can go to the next pass or not</summary>
		public abstract bool NextValid { get; }
		/// <summary>Proceed to the next pass</summary>
		public abstract (byte p, byte q, byte r) NextPass();
		/// <summary>Restarts the algorithm to the zeroth state</summary>
		public abstract void Reset();
	}
}