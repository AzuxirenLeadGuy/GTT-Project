using System.Collections.Generic;
using System.Text;

using Azuxiren.MG;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct FloydWarshallInfo
	{
		private readonly TextBox[,] _costMatrix;
		private readonly SpriteBatch _batch;
		public string LogText;
		private readonly byte _nodeCount;
		private readonly int _ux, _uy, _uw, _uh;
		private Rectangle _focus;
		private bool _isFocused;
		public FloydWarshallInfo(byte[,] edges, Rectangle region)
		{
			_nodeCount = (byte)edges.GetLength(0);
			_nodeCount++;
			_uw = region.Width / _nodeCount;
			_uh = region.Height / _nodeCount;
			int x = region.X, y = region.Y, i, j;
			_focus = new(x, y, _uw, _uh);
			_ux = x + _uw;
			_uy = y + _uh;
			_costMatrix = new TextBox[_nodeCount, _nodeCount];
			_nodeCount--;
			for (i = 0, y = _uy - _uh; i <= _nodeCount; i++, y += _uh)
			{
				for (j = 0, x = _ux - _uw; j <= _nodeCount; j++, x += _uw)
				{
					string text = i == 0 && j == 0 ? ""
						: i == 0 ? GameApp.GetLabel((byte)(j - 1))
						: j == 0 ? GameApp.GetLabel((byte)(i - 1)) : i == j ? "0"
						: (text = edges[i - 1, j - 1].ToString()) == "0" ? "INF"
						: text;
					_costMatrix[i, j] = new(new(x, y, _uw, _uh), text, GameApp.CommonData.Font, Color.White);
				}
			}
			_batch = GameApp.CommonData.Batch;
			LogText = "";
			_isFocused = false;
		}
		public void Update(FloydWarshallInfoUpdate update)
		{
			LogText = update.Log;
			if (update.DistUpdate.Key.From == 255)
			{
				_isFocused = false;
				return;
			}
			(byte from, byte to) = update.DistUpdate.Key;
			_isFocused = true;
			_focus.X = _ux + (to * _uw);
			_focus.Y = _uy + (from * _uh);
			_costMatrix[from + 1, to + 1].Text = update.DistUpdate.Distance.ToString();
		}
		public void Draw()
		{
			if (_isFocused) _batch.Draw(GameApp.CommonData.Patch, _focus, Color.Red);
			for (int i = 0; i <= _nodeCount; i++)
			{
				for (int j = 0; j <= _nodeCount; j++)
				{
					_costMatrix[i, j].Draw(_batch);
				}
			}
		}
	}
	public struct FloydWarshallInfoUpdate
	{
		public ((byte From, byte To) Key, uint Distance) DistUpdate;
		public string Log;
		public FloydWarshallInfoUpdate(string text)
		{
			Log = text;
			DistUpdate = ((255, 255), 0);
		}
		public FloydWarshallInfoUpdate(byte from, byte to, uint distance, string text)
		{
			DistUpdate = ((from, to), distance);
			Log = text;
		}
	}
}