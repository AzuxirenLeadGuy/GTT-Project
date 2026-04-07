using System.Collections.Generic;

using Microsoft.Xna.Framework;

using F_Updates = System.Collections.Generic.IEnumerable<GTT.FloydWarshallInfoUpdate>;
using G_Updates = System.Collections.Generic.IEnumerable<GTT.GraphDrawingUpdate>;

namespace GTT
{
	public static class Algorithms
	{
		public static string GetLabel(byte x) => ((char)((byte)'A' + x)).ToString();
		struct DisjointSet
		{
			public byte[] Parents, Ranks;
			public DisjointSet(byte size)
			{
				Parents = new byte[size];
				Ranks = new byte[size];
				for (byte i = 0; i < size; i++) Parents[i] = i;
			}
			public readonly byte GetParent(byte x)
				=> x == Parents[x] ? x : (Parents[x] = GetParent(Parents[x]));
			public readonly bool AreMerged(byte a, byte b)
				=> GetParent(a) == GetParent(b);
			public readonly void Merge(byte a, byte b)
			{
				a = GetParent(a);
				b = GetParent(b);
				if (a == b) return;
				if (Ranks[a] < Ranks[b])
					(a, b) = (b, a); // swap because Rank of a should be higher than b
				Parents[b] = a;
				if (Ranks[a] == Ranks[b])
					Ranks[a]++;
			}
		}
		public static G_Updates DepthFirstSearch(byte nodeCount, byte[,] edges, byte source, byte dest, Color[] allowedColors)
		{
			byte[] color = new byte[nodeCount];
			byte[] pred = new byte[nodeCount];
			int[] cost = new int[nodeCount];
			for (byte idx = 0; idx < nodeCount; idx++)
			{
				color[idx] = 0;
				pred[idx] = 255;
			}
			Stack<byte> stack = new();
			cost[source] = 0;
			stack.Push(source);
			do
			{
				byte u = stack.Peek();
				color[u] = 1;
				yield return new(u, allowedColors[2], $"Node {GetLabel(u)} is being explored");
				if (u == dest)
				{
					yield return new(u, allowedColors[4], $"Node {GetLabel(u)} is found!");
					string path = "";
					while (u != source)
					{
						path = $", {GetLabel(u)}" + path;
						u = pred[u];
					}
					path = $"{GetLabel(source)}{path}";
					yield return new(255, allowedColors[0], $"Total Cost={cost[dest]} for obtained path:\n{path}");
					yield break;
				}
				else
				{
					for (byte idx = 0; idx < nodeCount; idx++)
					{
						if (edges[u, idx] != 0 && color[idx] == 0) // if i is an unvisited neighbour
						{
							pred[idx] = u;
							cost[idx] = edges[u, idx] + cost[u];
							stack.Push(idx);
							yield return new(
								idx,
								allowedColors[1],
								$"Found neighbour {GetLabel(idx)}, with cost: {cost[idx]}."
							);
							goto flp;
						}
					}
					color[u] = 2;
					stack.Pop();
					yield return new(
						u,
						allowedColors[3],
						$"Node {GetLabel(u)} is fully explored!"
					);
				flp:;
				}
			} while (stack.Count > 0);
			if (dest != 255)
			{
				yield return new(
					255,
					allowedColors[0],
					$"Destination node cannot be reached"
				);
			}
			else
			{
				string costmap = $"{GetLabel(0)}: {cost[0]}";
				for (byte idx = 1; idx < nodeCount; idx++)
				{
					string cost_str = color[idx] == 0 ? "-" : cost[idx].ToString();
					costmap += $", {GetLabel(idx)}: {cost_str}";
				}
				yield return new(
					255,
					allowedColors[4],
					$"Cost of the entire graph:\n{costmap}"
				);
			}
			yield break;
		}
		public static G_Updates BreadthFirstSearch(byte nodeCount, byte[,] edges, byte source, byte dest, Color[] allowedColors)
		{
			byte[] color = new byte[nodeCount];
			byte[] pred = new byte[nodeCount];
			int[] cost = new int[nodeCount];
			for (byte idx = 0; idx < nodeCount; idx++)
			{
				color[idx] = 0;
				pred[idx] = 255;
			}
			Queue<byte> queue = new();
			queue.Enqueue(source);
			cost[source] = 0;
			do
			{
				byte u = queue.Peek();
				color[u] = 2;
				yield return new(u, allowedColors[2], $"Node {GetLabel(u)} is being explored");
				if (u == dest)
				{
					yield return new(u, allowedColors[4], $"Destination Node {GetLabel(u)} is found!");
					string path = "";
					while (u != source)
					{
						path = $", {GetLabel(u)}{path}";
						u = pred[u];
					}
					path = $"{GetLabel(source)}{path}";
					yield return new(
						255,
						allowedColors[0],
						$"Total Cost={cost[dest]} for obtained path:\n{path}"
					);
					yield break;
				}
				else
				{
					for (byte idx = 0; idx < nodeCount; idx++)
					{
						if (edges[u, idx] != 0 && color[idx] == 0) // if i is an unvisited neighbour
						{
							pred[idx] = u;
							cost[idx] = edges[u, idx] + cost[u];
							color[idx] = 1;
							queue.Enqueue(idx);
							yield return new(
								idx,
								allowedColors[1],
								$"Found neighbour {GetLabel(idx)} with cost: {cost[idx]}"
							);
						}
					}
					color[u] = 3;
					queue.Dequeue();
					yield return new(u, allowedColors[3], $"Node {GetLabel(u)} is fully explored!");
				}
			} while (queue.Count > 0);
			if (dest != 255)
			{
				yield return new(255, allowedColors[0], $"Destination node cannot be reached");
			}
			else
			{
				string costmap = $"{GetLabel(0)}: {cost[0]}";
				for (byte idx = 1; idx < nodeCount; idx++)
				{
					string cost_str = color[idx] == 0 ? "-" : cost[idx].ToString();
					costmap += $", {GetLabel(idx)}: {cost_str}";
				}
				yield return new(255, allowedColors[4], $"Cost of the entire graph:\n{costmap}");
			}
			yield break;
		}
		public static G_Updates Dijkstra(byte nodeCount, byte[,] edges, byte source, byte dest, Color[] allowedColors)
		{
			byte[] color = new byte[nodeCount];
			byte[] pred = new byte[nodeCount];
			int[] cost = new int[nodeCount];
			byte i;
			for (i = 0; i < nodeCount; i++)
			{
				color[i] = 0;
				pred[i] = 255;
				cost[i] = -1;
			}
			cost[source] = 0;
			PriorityQueue<byte, int> queue = new();
			queue.Enqueue(source, 0);
			do
			{
				byte u = queue.Dequeue();
				if (color[u] >= 2) continue;
				color[u] = 2;
				yield return new(u, allowedColors[2], $"Now exporing node {GetLabel(u)}");
				if (u == dest)
				{
					yield return new(u, allowedColors[4], $"Node {GetLabel(u)} is found.");
					string path = "";
					while (u != source)
					{
						path = $", {GetLabel(u)}" + path;
						u = pred[u];
					}
					path = $"[ {GetLabel(source)}" + path + " ]";
					yield return new(255, allowedColors[0], $"Cost is {cost[dest]} of Path \n{path} ");
					queue.Clear();
					yield break;
				}
				else
				{
					for (i = 0; i < nodeCount; i++)
					{
						if (edges[u, i] > 0 && color[i] < 2) // A neigbour node that is not explored yet
						{
							if (cost[i] == -1 || edges[u, i] + cost[u] < cost[i]) // This path is better than already explored
							{
								cost[i] = edges[u, i] + cost[u];
								yield return new(i, allowedColors[1], $"Found{(color[i] == 0 ? " " : " a better ")}path to neighbour {GetLabel(i)} with cost {cost[i]}");
								if (color[i] == 0)
									queue.Enqueue(i, cost[i]);
								else
									color[i] = 1;
								pred[i] = u;
							}
						}
					}
					yield return new(u, allowedColors[3], $"Node {GetLabel(u)} is completely expored");
				}
			} while (queue.Count > 0);
			if (dest != 255)
			{
				yield return new(255, allowedColors[0], $"Destination node cannot be reached");
			}
			string x = $" {GetLabel(0)}:{(cost[0] == -1 ? "INF" : cost[0].ToString())}";
			for (i = 1; i < nodeCount; i++)
			{
				x += $", {GetLabel(i)}:{(cost[i] == -1 ? "INF" : cost[i].ToString())}";
			}
			yield return new(255, allowedColors[0], $"The cost vector is \n{x} ");
			yield break;
		}
		public static G_Updates Kruskal(byte nodeCount, byte[,] edges, Color[] allowedColors)
		{
			DisjointSet sets = new(nodeCount);
			SortedSet<(byte Weight, byte From, byte To)> edgeSet = [];
			byte i, j;
			nodeCount--;
			for (i = 0; i < nodeCount; i++)
			{
				for (j = (byte)(i + 1); j <= nodeCount; j++)
				{
					if (edges[i, j] > 0) edgeSet.Add((edges[i, j], i, j));
				}
			}
			yield return new(255, allowedColors[0], "Going through all edges in increasing order of weights");
			i = 0;
			int cost = 0;
			foreach (var (weight, from, to) in edgeSet)
			{
				string text = $"({GetLabel(from)},{GetLabel(to)})";
				yield return new GraphDrawingUpdate(from, to, allowedColors[2], $"Checking edge {text} ...");
				if (sets.AreMerged(from, to))
					yield return new(from, to, allowedColors[5], $"Adding this edge will introduce a cycle\nEdge {text} is discarded");
				else
				{
					yield return new(from, to, allowedColors[4], $"This edge can be included in the MST.\n Edge {text} is included");
					cost += weight;
					i++;
					sets.Merge(from, to);
				}
				if (i == nodeCount)
				{
					yield return new(255, allowedColors[4], $"MST of cost {cost} is formed with the edges marked with green color");
					yield break;
				}
			}
			yield return new(255, allowedColors[0], $"Cannot select {nodeCount} edges to make a MST\nThe graph is not connected");
			yield break;
		}
		public static F_Updates FloydWarshall(byte nodeCount, byte[,] edges)
		{
			uint[,] matrix = new uint[nodeCount, nodeCount];
			byte i, j, k;
			for (i = 0; i < nodeCount; i++)
			{
				for (j = 0; j < nodeCount; j++)
				{
					matrix[i, j] = edges[i, j];
				}
			}
			yield return new("Initializing the matrix from the adjacency matrix");
			for (i = 0; i < nodeCount; i++)
			{
				yield return new($"Pass {i + 1}");
				for (j = 0; j < nodeCount; j++)
				{
					if (i == j) continue;
					for (k = 0; k < nodeCount; k++)
					{
						if (k == i || k == j || matrix[i, k] == 0 || matrix[j, i] == 0) continue;
						uint newdist = matrix[j, i] + matrix[i, k];
						uint olddist = matrix[j, k];
						if (olddist == 0 || newdist < olddist)
						{
							matrix[j, k] = newdist;
							yield return new(j, k, newdist, $"Updated distance between {GetLabel(j)} and {GetLabel(k)} to {newdist} instead of {(olddist != 0 ? olddist.ToString() : "INF")}");
						}
					}
				}
			}
			yield return new("Matrix for all pairs-shortest distance is ready.");
		}
	}
}