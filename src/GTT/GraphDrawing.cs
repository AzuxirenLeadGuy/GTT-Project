using System.Collections.Generic;

using Azuxiren.MG;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct GraphDrawing
	{
		public (TextBox Box, Color color)[] Nodes;
		public Dictionary<(byte From, byte To), LineObject> Edges;
		private readonly SpriteBatch _batch;
		private readonly Texture2D _patch;
		public string Log;
		public GraphDrawing(Rectangle[] pos, Dictionary<(byte From, byte To), LineObject> edges)
		{
			int i, l = pos.Length;
			Nodes = new (TextBox Box, Color color)[l];
			for (i = 0; i < l; i++) Nodes[i] = (new TextBox(pos[i], GameApp.GetLabel((byte)i), GameApp.CommonData.Font, Color.Black), Color.White);
			Edges = edges;
			_batch = GameApp.CommonData.Batch;
			_patch = GameApp.CommonData.Circle;
			Log = "";
		}
		public void Draw()
		{
			foreach (var edge in Edges)
			{
				edge.Value.Draw(_batch);
			}
			for (int i = Nodes.Length - 1; i >= 0; i--)
			{
				_batch.Draw(_patch, Nodes[i].Box.Bounds, Nodes[i].color);
				Nodes[i].Box.Draw(_batch);
			}
		}
		public void Reset()
		{
			for (int i = Nodes.Length - 1; i >= 0; i--)
			{
				Nodes[i].color = Color.White;
				Nodes[i].Box.TextColor = Color.Black;
			}
			foreach (var edge in Edges)
			{
				edge.Value.ArrowColor = Color.Yellow;
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