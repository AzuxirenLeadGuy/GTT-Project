using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct FloydWarshallInfo
	{
		private readonly TextBox[,] _costMatrix;
		public string LogText;
		private Color _focusColor;
		private readonly byte _nodeCount;
		private readonly int _ux, _uy, _uw, _uh;
		private Rectangle _focus;
		private bool _isFocused;
		private readonly Texture2D _patch;
		public FloydWarshallInfo(
			byte[,] edges,
			Rectangle region,
			CommonDataStruct settings
		)
		{
			_focusColor = settings.ComponentHoverColor;
			_patch = settings.Patch;
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
						: i == 0 ? Algorithms.GetLabel((byte)(j - 1))
						: j == 0 ? Algorithms.GetLabel((byte)(i - 1)) : i == j ? "0"
						: (text = edges[i - 1, j - 1].ToString()) == "0" ? "-"
						: text;
					_costMatrix[i, j] = new(
						new(x, y, _uw, _uh),
						text,
						settings.Font,
						settings.ComponentTextColor
					);
				}
			}
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
		public readonly void Draw(IBatchDrawer batch)
		{
			if (_isFocused) batch.Draw(_patch, _focus, color: _focusColor);
			for (int i = 0; i <= _nodeCount; i++)
			{
				for (int j = 0; j <= _nodeCount; j++)
				{
					_costMatrix[i, j].Draw(batch);
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