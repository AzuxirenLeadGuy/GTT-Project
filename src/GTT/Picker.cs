using System;
using Azuxiren.MG;
using Azuxiren.MG.Menu;
using Microsoft.Xna.Framework;

namespace GTT
{
	public class Picker : IMenuItem
	{
		private readonly Button _next, _prev;
		private TextBox _textbox;
		private readonly string[] _choices;
		public byte Index { get; private set; }
		private readonly byte _limit;
		public class SelectArgs : EventArgs
		{
			public byte Prev, Curr;
		}
		public event EventHandler<SelectArgs>? SelectionChanged;
		public Picker(string[] choice, Rectangle bds)
		{
			_choices = choice ?? throw new ArgumentException("string array cannot be null!", nameof(choice));
			int w_20 = bds.Width / 5;
			_next = new Button(new Rectangle(bds.X + 4 * w_20, bds.Y, w_20, bds.Height), ">");
			_prev = new Button(new Rectangle(bds.X, bds.Y, w_20, bds.Height), "<");
			_textbox = new TextBox(new Rectangle(bds.X + w_20, bds.Y, 3 * w_20, bds.Height), _choices[0], GameApp.CommonData.Font)
			{
				Alignment = Alignment.Centered
			};
			Index = 0;
			_limit = (byte)(_choices.Length - 1);
			_next.OnRelease += Next;
			_prev.OnRelease += Prev;
		}
		private void Next(object? o, ComponentArgs a)
		{
			if (Index >= _limit) return;
			byte past = Index++;
			_textbox.Text = _choices[Index];
			SelectionChanged?.Invoke(this, new SelectArgs() { Prev = past, Curr = Index });
		}
		private void Prev(object? o, ComponentArgs a)
		{
			if (Index == 0) return;
			byte past = Index;
			_textbox.Text = _choices[--Index];
			SelectionChanged?.Invoke(this, new SelectArgs() { Prev = past, Curr = Index });
		}
		public void Set(Rectangle bds)
		{
			int w_20 = bds.Width / 5;
			(_next.Bounds.X, _next.Bounds.Y, _next.Bounds.Width, _next.Bounds.Height) = (bds.X + 4 * w_20, bds.Y, w_20, bds.Height);
			(_prev.Bounds.X, _prev.Bounds.Y, _prev.Bounds.Width, _prev.Bounds.Height) = (bds.X, bds.Y, w_20, bds.Height);
			_textbox.Bounds = new Rectangle(bds.X + w_20, bds.Y, 3 * w_20, bds.Height);
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
			_textbox.Draw(GameApp.CommonData.Batch);
		}
	}
}