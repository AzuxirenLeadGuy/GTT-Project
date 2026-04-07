using System.Collections.Generic;

using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct GraphDrawing
	{
		public (TextBox Box, Color color)[] Nodes;
		public Dictionary<(byte From, byte To), LineObject> Edges;
		private readonly Texture2D _patch;
		private Color _base, _node, _arrow;
		public string Log;
		public GraphDrawing(
			Rectangle[] pos,
			Dictionary<(byte From, byte To), LineObject> edges,
			CommonDataStruct settings)
		{
			int i, l = pos.Length;
			Nodes = new (TextBox Box, Color color)[l];
			for (i = 0; i < l; i++) Nodes[i] = (
				new TextBox(
					pos[i],
					Algorithms.GetLabel((byte)i),
					settings.Font,
					settings.ComponentTextColor
				),
				settings.ComponentUnselectedColor
			);
			Edges = edges;
			_patch = settings.Circle;
			Log = "";
			_base = settings.GDBaseColor;
			_arrow = settings.GDArrowColor;
			_node = settings.GDNodeColor;
		}
		public readonly void Draw(IBatchDrawer batch)
		{
			foreach (var edge in Edges)
			{
				edge.Value.Draw(batch);
			}
			for (int i = Nodes.Length - 1; i >= 0; i--)
			{
				batch.Draw(_patch, destination: Nodes[i].Box.Bounds, color: Nodes[i].color);
				Nodes[i].Box.Draw(batch);
			}
		}
		public readonly void Reset()
		{
			for (int i = Nodes.Length - 1; i >= 0; i--)
			{
				Nodes[i].color = _base;
				Nodes[i].Box.TextColor = _node;
			}
			foreach (var edge in Edges)
			{
				edge.Value.ArrowColor = _arrow;
			}
		}
		public void Update(GraphDrawingUpdate update)
		{
			if (update.NodeUpdate.Node != 255)
				Nodes[update.NodeUpdate.Node].color = update.NodeUpdate.color;
			if (update.EdgeUpdate.Key.From != 255)
				Edges[update.EdgeUpdate.Key].ArrowColor = update.EdgeUpdate.color;
			Log = update.UpdateLog;
		}
	}
	public struct GraphDrawingUpdate
	{
		public (byte Node, Color color) NodeUpdate;
		public ((byte From, byte To) Key, Color color) EdgeUpdate;
		public string UpdateLog;
		public GraphDrawingUpdate(byte node, Color color, string log)
		{
			NodeUpdate = (node, color);
			EdgeUpdate = ((255, 255), color);
			UpdateLog = log;
		}
		public GraphDrawingUpdate(byte from, byte to, Color color, string log)
		{
			NodeUpdate = (255, color);
			EdgeUpdate = ((from, to), color);
			UpdateLog = log;
		}
	}
}