using System;

using Azuxiren.MG;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public class Picker : IMenuItem
	{
		private readonly static SpriteBatch Batch = GameApp.CurrentGame.SpriteBatch;
		private readonly Button _next, _prev;
		private TextBox _textbox;
		private readonly string[] _choices;
		private byte _index;
		private readonly byte _limit;
		public class SelectArgs : EventArgs
		{
			public byte Prev, Curr;
		}
		public event EventHandler<SelectArgs> SelectionChanged;
		public Picker(string[] choice, Rectangle bds)
		{
			_choices = choice;
			int w_20 = bds.Width / 5;
			_next = new Button(new Rectangle(bds.X + 4 * w_20, bds.Y, w_20, bds.Height), ">");
			_prev = new Button(new Rectangle(bds.X, bds.Y, w_20, bds.Height), "<");
			_textbox = new TextBox(new Rectangle(bds.X + w_20, bds.Y, 3 * w_20, bds.Height), _choices[0], GameApp.CurrentGame.Font)
			{
				Alignment = Alignment.Centered
			};
			_index = 0;
			_limit = (byte)(_choices.Length - 1);
		}
		private void Next(object o, ComponentArgs a)
		{
			if (_index >= _limit) return;
			byte past = _index++;
			_textbox.Text = _choices[_index];
			SelectionChanged?.Invoke(this, new SelectArgs() { Prev = past, Curr = _index });
		}
		private void Prev(object o, ComponentArgs a)
		{
			if (_index == 0) return;
			byte past = _index;
			_textbox.Text = _choices[--_index];
			SelectionChanged?.Invoke(this, new SelectArgs() { Prev = past, Curr = _index });
		}
		public void Set(Rectangle bds)
		{
			int w_20 = bds.Width / 5;
			(_next.Bounds.X, _next.Bounds.Y, _next.Bounds.Width, _next.Bounds.Height) = (bds.X + 4 * w_20, bds.Y, w_20, bds.Height);
			(_prev.Bounds.X, _prev.Bounds.Y, _prev.Bounds.Width, _prev.Bounds.Height) = (bds.X, bds.Y, w_20, bds.Height);
			_textbox.Bounds = new Rectangle(bds.X + w_20, bds.Y, 3 * w_20, bds.Height);
		}
		public void LoadContent()
		{
			_next.LoadContent();
			_prev.LoadContent();
			_next.OnRelease += Next;
			_prev.OnRelease += Prev;
		}
		public void Update(GameTime gt)
		{
			_next.Update(gt);
			_prev.Update(gt);
		}
		public void Draw(GameTime gt)
		{
			_next.Draw(gt);
			_prev.Draw(gt);
			_textbox.Draw(Batch);
		}
	}
}