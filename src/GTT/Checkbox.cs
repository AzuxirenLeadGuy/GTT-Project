using Azuxiren.MG.Drawing;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public class Checkbox : BaseSwitch, IMenuItem
	{
		public bool IsChecked => SwitchedOn ?? false;
		protected Rectangle _box, _tickmark;
		protected TextBox _textBox;
		private readonly Texture2D _patch;
		protected IInputManager _input;
		protected Color _hoverColor, _baseColor;
		public bool Hover => _box.Contains(_input.PointerLocation);
		public override bool Press => Hover && _input.Clicked;
		public override bool Enabled => true;
		public override BaseSwitchState State { get; protected set; }
		public Checkbox(CommonDataStruct settings, string message, Rectangle? bounds = null, bool check = false)
		{
			_input = settings.Input;
			_textBox = new(
				Rectangle.Empty,
				message,
				settings.Font,
				settings.ComponentTextColor
			);
			_patch = settings.Patch;
			State = check ? BaseSwitchState.ReleasedOn : BaseSwitchState.ReleasedOff;
			if (bounds != null)
			{
				Set(bounds.Value);
			}
			_hoverColor = settings.ComponentHoverColor;
			_baseColor = settings.ComponentUnselectedColor;
		}
		public void Set(Rectangle bds)
		{
			_box = bds;
			_box.Width = _box.Height;
			_tickmark = DrawingExtensions.SetCenter(
				_box.Center,
				new(_box.Width / 2, _box.Height / 2)
			);
			_textBox.Bounds = new Rectangle(
				_box.Right,
				_box.Top,
				bds.Width - _box.Width,
				_box.Height
			);
		}

		public override void Draw(IBatchDrawer drawer)
		{
			drawer.Draw(
				_patch,
				_box,
				color: Hover ? _hoverColor : _baseColor
			);
			if (IsChecked)
			{
				drawer.Draw(_patch, _tickmark);
			}
			_textBox.Draw(drawer);
		}
	}
}