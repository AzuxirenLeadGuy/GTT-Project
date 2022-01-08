using System.Collections.Generic;
namespace GraphAlgos
{
	/// <summary>
	/// Represents a Graph abstract structure
	/// </summary>
	public class Graph
	{
		/// <summary>
		/// Denotes that the graph edges are weighted or unweighted
		/// </summary>
		/// <value>bool</value>
		public bool Weighted { get; private set; }
		/// <summary>
		/// Denotes that the graph edges are directed or undirected
		/// </summary>
		/// <value>bool</value>
		public bool Directed { get; private set; }
		/// <summary>
		/// Denotes the number of vertices in the graph
		/// </summary>
		/// <value>byte</value>
		public byte VerticesCount => _vc;
		private readonly byte _vc;
		private readonly byte[,] AdjMat;
		/// <summary>
		/// Creates a graph
		/// </summary>
		/// <param name="vertices">The number of vertices that should be present in graph</param>
		/// <param name="weighted">If graph should be weighted</param>
		/// <param name="directed">If graph should be directed</param>
		public Graph(byte vertices, bool weighted, bool directed)
		{
			_vc = vertices;
			Weighted = weighted;
			Directed = directed;
			AdjMat = new byte[_vc, _vc];
		}
		/// <summary>
		/// Returns the weight of edge between the two vertices
		/// </summary>
		/// <param name="p">Vertex of the graph</param>
		/// <param name="q">Vertex of the graph</param>
		/// <returns>Weight of the edge</returns>
		internal byte GetWeight(byte p, byte q) => AdjMat[p, q];
		private void Set(byte p, byte q, byte weight) => AdjMat[p, q] = weight;
		/// <summary>
		/// Sets weight for a given edge
		/// </summary>
		/// <param name="p">The vertex from which the edge starts</param>
		/// <param name="q">The vertex on which the edge ends</param>
		/// <param name="weight">The weight to set. 
		/// Setting weight 0 is equivalent of removing the edge.
		/// For unweighted graph. setting any positive value will create an edge  
		/// </param>
		/// <returns></returns>
		public bool SetWeight(byte p, byte q, byte weight)
		{
			if (p >= _vc || q >= _vc) return false;
			if (!Weighted && weight != 0) weight = 1;
			Set(p, q, weight);
			if (!Directed) Set(q, p, weight);
			return true;
		}
		/// <summary>
		/// Denotes wheter two given vertices are adjacent to each other
		/// </summary>
		/// <param name="p">Vertex of the graph</param>
		/// <param name="q">Vertex of the graph</param>
		/// <returns>Returns true if the vertices are adjacent,
		/// otherwise false</returns>
		internal bool Adjacent(byte p, byte q) => p < _vc && q < _vc && AdjMat[p, q] != 0;
		/// <summary>
		/// Iterates all the neigbours of a given vertex 
		/// </summary>
		/// <param name="v">Vertex of the graph</param>
		/// <param name="outgoing">If true, iterates all vertices q s.t v->q
		/// , else if false, iterates all vertices q s.t q->v</param>
		/// <returns>Returns the Iteration of all neigbours
		/// of the vertex</returns>
		public IEnumerable<byte> NeighboursOf(byte v, bool outgoing = true)
		{
			if (outgoing)
			{
				for (byte i = 0; i < _vc; i++)
				{
					if (Adjacent(v, i)) yield return i;
				}
			}
			else
			{
				for (byte i = 0; i < _vc; i++)
				{
					if (Adjacent(i, v)) yield return i;
				}
			}
			yield break;
		}
	}
}