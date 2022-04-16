using System.Collections.Generic;
using G_Updates = System.Collections.Generic.IEnumerable<GTT.GraphDrawingUpdate>;
using F_Updates = System.Collections.Generic.IEnumerable<GTT.FloydWarshallInfoUpdate>;
using Microsoft.Xna.Framework;

namespace GTT
{
	public static class Algorithms
	{
		public readonly static Color[] AllowedColors = { Color.White, Color.LightPink, Color.Cyan, Color.Orange, Color.LimeGreen, Color.Red };
		public static G_Updates DepthFirstSearch(byte nodeCount, byte[,] edges, byte source, byte dest)
		{
			byte[] color = new byte[nodeCount];
			byte[] pred = new byte[nodeCount];
			byte i;
			for (i = 0; i < nodeCount; i++)
			{
				color[i] = 0;
				pred[i] = 255;
			}
			Stack<byte> stack = new();
			stack.Push(source);
			do
			{
				byte u = stack.Peek();
				color[u] = 1;
				yield return new(u, AllowedColors[2], $"Node {GameApp.GetLabel(u)} is being explored");
				if (u == dest)
				{
					yield return new(u, AllowedColors[4], $"Node {GameApp.GetLabel(u)} is found!");
					int cost = 0;
					string path = "";
					while (u != source)
					{
						path = $", {GameApp.GetLabel(u)}" + path;
						cost += edges[pred[u], u];
						u = pred[u];
					}
					path = $"[ {GameApp.GetLabel(source)}" + path + " ]";
					yield return new(255, AllowedColors[0], $"Total Cost={cost} for obtained path:\n{path}");
					yield break;
				}
				else
				{
					for (i = 0; i < nodeCount; i++)
					{
						if (edges[u, i] != 0 && color[i] == 0) // if i is an unvisited neighbour
						{
							pred[i] = u;
							stack.Push(i);
							yield return new(i, AllowedColors[1], $"Found neighbour {GameApp.GetLabel(i)}.");
							goto flp;
						}
					}
					color[u] = 2;
					stack.Pop();
					yield return new(u, AllowedColors[3], $"Node {GameApp.GetLabel(u)} is fully explored!");
				flp:;
				}
			} while (stack.Count > 0);
			yield return new(255, AllowedColors[0], $"Destination node cannot be reached");
			yield break;
		}
		public static G_Updates BreadthFirstSearch(byte nodeCount, byte[,] edges, byte source, byte dest)
		{
			byte[] color = new byte[nodeCount];
			byte[] pred = new byte[nodeCount];
			byte i;
			for (i = 0; i < nodeCount; i++)
			{
				color[i] = 0;
				pred[i] = 255;
			}
			Queue<byte> queue = new();
			queue.Enqueue(source);
			do
			{
				byte u = queue.Peek();
				color[u] = 2;
				yield return new(u, AllowedColors[2], $"Node {GameApp.GetLabel(u)} is being explored");
				if (u == dest)
				{
					yield return new(u, AllowedColors[4], $"Node {GameApp.GetLabel(u)} is found!");
					int cost = 0;
					string path = "";
					while (u != source)
					{
						path = $", {GameApp.GetLabel(u)}" + path;
						cost += edges[pred[u], u];
						u = pred[u];
					}
					path = $"[ {GameApp.GetLabel(source)}" + path + " ]";
					yield return new(255, AllowedColors[0], $"Total Cost={cost} for obtained path:\n{path}");
					yield break;
				}
				else
				{
					for (i = 0; i < nodeCount; i++)
					{
						if (edges[u, i] != 0 && color[i] == 0) // if i is an unvisited neighbour
						{
							pred[i] = u;
							color[i] = 1;
							queue.Enqueue(i);
							yield return new(i, AllowedColors[1], $"Found neighbour {GameApp.GetLabel(i)}.");
						}
					}
					color[u] = 3;
					queue.Dequeue();
					yield return new(u, AllowedColors[3], $"Node {GameApp.GetLabel(u)} is fully explored!");
				}
			} while (queue.Count > 0);
			yield return new(255, AllowedColors[0], $"Destination node cannot be reached");
			yield break;
		}
		public static G_Updates Dijkstra(byte nodeCount, byte[,] edges, byte source, byte dest)
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
				if(color[u]>=2) continue;
				color[u] = 2;
				yield return new(u, AllowedColors[2], $"Now exporing node {GameApp.GetLabel(u)}");
				if (u == dest)
				{
					yield return new(u, AllowedColors[4], $"Node {GameApp.GetLabel(u)} is found.");
					string path = "";
					while (u != source)
					{
						path = $", {GameApp.GetLabel(u)}" + path;
						u = pred[u];
					}
					path = $"[ {GameApp.GetLabel(source)}" + path + " ]";
					yield return new(255, AllowedColors[0], $"Cost is {cost[dest]} of Path \n{path} ");
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
								yield return new(i, AllowedColors[1], $"Found{(color[i] == 0 ? " " : " a better ")}path to neighbour {GameApp.GetLabel(i)} with cost {cost[i]}");
								if (color[i] == 0)
									queue.Enqueue(i, cost[i]);
								else
									color[i] = 1;
								pred[i] = u;
							}
						}
					}
					yield return new(u, AllowedColors[3], $"Node {GameApp.GetLabel(u)} is completely expored");
				}
			} while (queue.Count > 0);
			yield return new(255, AllowedColors[0], "The destination cannot be reached");
			string x = $"[ {(cost[0] == -1 ? "INF" : cost[0].ToString())}";
			for (i = 1; i < nodeCount; i++) x += $", {(cost[i] == -1 ? "INF" : cost[i].ToString())}";
			x += " ]";
			yield return new(255, AllowedColors[0], $"The cost vector is \n{x}");
			yield break;
		}
		public static G_Updates Kruskal(byte nodeCount, byte[,] edges)
		{
			DisjointSet sets = new(nodeCount);
			SortedSet<(byte Weight, byte From, byte To)> edgeSet = new();
			byte i, j;
			nodeCount--;
			for (i = 0; i < nodeCount; i++)
			{
				for (j = (byte)(i + 1); j <= nodeCount; j++)
				{
					if (edges[i, j] > 0) edgeSet.Add((edges[i, j], i, j));
				}
			}
			yield return new(255, AllowedColors[0], "Going through all edges in increasing order of weights");
			i = 0;
			int cost = 0;
			foreach (var (weight, from, to) in edgeSet)
			{
				string text = $"({GameApp.GetLabel(from)},{GameApp.GetLabel(to)})";
				yield return new GraphDrawingUpdate(from, to, AllowedColors[2], $"Checking edge {text} ...");
				if (sets.AreMerged(from, to))
					yield return new(from, to, AllowedColors[5], $"Adding this edge will introduce a cycle\nEdge {text} is discarded");
				else
				{
					yield return new(from, to, AllowedColors[4], $"This edge can be included in the MST.\n Edge {text} is included");
					cost += weight;
					i++;
					sets.Merge(from, to);
				}
				if (i == nodeCount)
				{
					yield return new(255, AllowedColors[4], $"MST of cost {cost} is formed with the edges marked with green color");
					yield break;
				}
			}
			yield return new(255, AllowedColors[0], $"Cannot select {nodeCount} edges to make a MST\nThe graph is not connected");
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
							yield return new(j, k, newdist, $"Updated distance between {GameApp.GetLabel(j)} and {GameApp.GetLabel(k)} to {newdist} instead of {(olddist != 0 ? olddist.ToString() : "INF")}");
						}
					}
				}
			}
			yield return new("Matrix for all pairs-shortest distance is ready.");
		}
	}
	public struct DisjointSet
	{
		public byte[] Parents, Ranks;
		public DisjointSet(byte size)
		{
			Parents = new byte[size];
			Ranks = new byte[size];
			for (byte i = 0; i < size; i++) Parents[i] = i;
		}
		public byte GetParent(byte x) => x == Parents[x] ? x : (Parents[x] = GetParent(Parents[x]));
		public bool AreMerged(byte a, byte b) => GetParent(a) == GetParent(b);
		public void Merge(byte a, byte b)
		{
			a = GetParent(a);
			b = GetParent(b);
			if (a == b) return;
			if (Ranks[a] < Ranks[b])
				(a, b) = (b, a);
			Parents[b] = a;
			if (Ranks[a] == Ranks[b])
				Ranks[a]++;
		}
	}
}