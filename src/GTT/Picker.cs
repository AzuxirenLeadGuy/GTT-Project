using System;

using Azuxiren.MG.Drawing;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public class Picker : IMenuItem
	{
		private readonly Button _next, _prev;
		private readonly TextBox _textbox;
		private readonly string[] _choices;
		public byte Index { get; private set; }
		protected Rectangle _bound;
		private byte _limit;
		protected Texture2D _patch;
		protected Color _backColor;
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
				settings.ComponentTextColor,
				TextBox.TextAlignment.Centered
			);
			_backColor = settings.GDArrowColor;
			_patch = settings.Patch;
			Index = 0;
			_limit = (byte)(_choices.Length - 1);
			UpdateButtonEnableProp();
		}
		public void ResetIndex()
		{
			Index = 0;
			_textbox.Text = _choices[Index];
		}

		public bool SetLimit(byte value)
		{
			if (value >= _choices.Length) { return false; }
			_limit = value;
			ResetIndex();
			UpdateButtonEnableProp();
			return true;
		}
		private void UpdateButtonEnableProp()
		{
			_prev.EnabledProperty = Index > 0;
			_next.EnabledProperty = Index < _limit;
		}
		private SelectArgs? NextResult()
		{
			if (Index >= _limit) return null;
			byte past = Index++;
			_textbox.Text = _choices[Index];
			UpdateButtonEnableProp();
			return new SelectArgs() { Prev = past, Curr = Index };
		}
		private SelectArgs? PrevResult()
		{
			if (Index == 0) return null;
			byte past = Index--;
			_textbox.Text = _choices[Index];
			UpdateButtonEnableProp();
			return new SelectArgs() { Prev = past, Curr = Index };
		}
		public void Set(Rectangle bds)
		{
			int w_20 = bds.Width / 5;
			_next.Set(new(bds.X + 4 * w_20, bds.Y, w_20, bds.Height));
			_prev.Set(new(bds.X, bds.Y, w_20, bds.Height));
			_textbox.Bounds = new Rectangle(bds.X + w_20, bds.Y, 3 * w_20, bds.Height);
		}
		public SelectArgs? Update(GameTime gt)
		{
			var next_update = _next.Update(gt);
			var prev_update = _prev.Update(gt);
			return
				next_update == BaseButton.BaseButtonState.JustPressed ? NextResult() :
				prev_update == BaseButton.BaseButtonState.JustPressed ? PrevResult() :
				null;
		}
		public void Draw(IBatchDrawer drawer)
		{
			_next.Draw(drawer);
			_prev.Draw(drawer);
			drawer.Draw(_patch, _textbox.Bounds, color: _backColor);
			_textbox.Draw(drawer);
		}
	}
}