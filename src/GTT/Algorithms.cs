using System.Collections.Generic;
using G_Updates = System.Collections.Generic.IEnumerable<GTT.GraphDrawingUpdate>;
using F_Updates = System.Collections.Generic.IEnumerable<GTT.FloydWarshallInfoUpdate>;
using Microsoft.Xna.Framework;

namespace GTT
{
	public static class Algorithms
	{
		public readonly static Color[] AllowedColors = { Color.White, Color.LightPink, Color.Cyan, Color.Orange, Color.LimeGreen };
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
			string x = $"[ {cost[0]}";
			for (i = 1; i < nodeCount; i++) x += $", {cost[i]}";
			x += " ]";
			yield return new(255, AllowedColors[0], $"The cost vector is \n{x}");
			yield break;
		}
	}
}