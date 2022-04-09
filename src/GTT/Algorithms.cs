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
					stack.Clear();
					int cost = 0;
					do
					{
						stack.Push(u);
						u = pred[u];
					} while (u != 255);
					string path = $"[ {GameApp.GetLabel(stack.Pop())}";
					do
					{
						u = stack.Pop();
						cost += edges[source, u];
						source = u;
						path += $", {GameApp.GetLabel(u)} ";
					} while (stack.Count > 0);
					path += " ]";
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
					Stack<byte> stack = new();
					queue.Clear();
					int cost = 0;
					do
					{
						stack.Push(u);
						u = pred[u];
					} while (u != 255);
					string path = $"[ {GameApp.GetLabel(stack.Pop())}";
					do
					{
						u = stack.Pop();
						cost += edges[source, u];
						source = u;
						path += $", {GameApp.GetLabel(u)} ";
					} while (stack.Count > 0);
					path += " ]";
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
	}
}