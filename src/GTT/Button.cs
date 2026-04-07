using Azuxiren.MG.Drawing;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public class Button(CommonDataStruct settings, string message = "", Rectangle? bounds = null, bool circle = false) : BaseButton, IMenuItem
	{
		private readonly TextBox _button_text = new(bounds ?? Rectangle.Empty, message, settings.Font, settings.ComponentTextColor);
		private readonly Texture2D _patch = circle ? settings.Circle : settings.Patch;
		public override bool Press => Hover && _input.Clicked;
		public bool EnabledProperty = true;
		public override bool Enabled { get => EnabledProperty; }
		public bool Hover => Bounds.Contains(_input.PointerLocation);
		public override BaseButtonState State { get; protected set; }
		protected internal Rectangle _bounds = bounds ?? Rectangle.Empty;
		public override Rectangle Bounds => _bounds;
		protected readonly Color[] _palette = settings.ComponentPalette;
		protected IInputManager _input = settings.Input;

		public override void Draw(IBatchDrawer drawer)
		{
			Color backcolor = State switch
			{
				BaseButtonState.Released => Hover ? _palette[1] : _palette[0],
				BaseButtonState.JustPressed => _palette[2],
				BaseButtonState.Pressed => _palette[2],
				BaseButtonState.JustReleased => _palette[2],
				_ => _palette[3],
			};
			drawer.Draw(_patch, destination: _bounds, color: backcolor);
			_button_text.Draw(drawer);
		}
		public void Set(Rectangle bds)
		{
			_bounds = bds;
			_button_text.Bounds = bds;
		}
	}
}