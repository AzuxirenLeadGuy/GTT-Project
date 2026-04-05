using System;

using Azuxiren.MG.Drawing;
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
		protected Rectangle _bound;
		private readonly byte _limit;
		public class SelectArgs : EventArgs
		{
			public byte Prev, Curr;
		}
		public Picker(string[] choice, Rectangle bds, CommonDataStruct settings)
		{
			_choices = choice ?? throw new ArgumentException("string array cannot be null!", nameof(choice));
			int w_20 = bds.Width / 5;
			_next = new Button(settings, ">", new Rectangle(bds.X + 4 * w_20, bds.Y, w_20, bds.Height));
			_prev = new Button(settings, "<", new Rectangle(bds.X, bds.Y, w_20, bds.Height));
			_textbox = new TextBox(
				new Rectangle(bds.X + w_20, bds.Y, 3 * w_20, bds.Height),
				_choices[0],
				settings.Font,
				Color.Black,
				TextBox.TextAlignment.Centered
			);
			Index = 0;
			_limit = (byte)(_choices.Length - 1);
		}
		private SelectArgs? NextResult()
		{
			if (Index >= _limit) return null;
			byte past = Index++;
			_textbox.Text = _choices[Index];
			return new SelectArgs() { Prev = past, Curr = Index };
		}
		private SelectArgs? PrevResult()
		{
			if (Index == 0) return null;
			byte past = Index;
			_textbox.Text = _choices[--Index];
			return new SelectArgs() { Prev = past, Curr = Index };
		}
		public void Set(Rectangle bds)
		{
			int w_20 = bds.Width / 5;
			_next.Set(new(bds.X + 4 * w_20, bds.Y, w_20, bds.Height));
			_prev.Set(new(bds.X, bds.Y, w_20, bds.Height));
			// (_next.Bounds.X, _next.Bounds.Y, _next.Bounds.Width, _next.Bounds.Height) = (bds.X + 4 * w_20, bds.Y, w_20, bds.Height);
			// (_prev.Bounds.X, _prev.Bounds.Y, _prev.Bounds.Width, _prev.Bounds.Height) = (bds.X, bds.Y, w_20, bds.Height);
			_textbox.Bounds = new Rectangle(bds.X + w_20, bds.Y, 3 * w_20, bds.Height);
		}
		public SelectArgs? Update(GameTime gt)
		{
			var next_update = _next.Update(gt);
			var prev_update = _prev.Update(gt);
			return
				next_update == BaseButton.BaseButtonState.JustReleased ? NextResult() :
				prev_update == BaseButton.BaseButtonState.JustReleased ? PrevResult() :
				null;
		}
		public void Draw(IBatchDrawer drawer)
		{
			_next.Draw(drawer);
			_prev.Draw(drawer);
			_textbox.Draw(drawer);
		}
	}
}