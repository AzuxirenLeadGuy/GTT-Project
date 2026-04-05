using Azuxiren.MG.Drawing;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public class Checkbox : BaseSwitch, IMenuItem
	{
		protected bool _isChecked;
		public bool IsChecked => _isChecked;
		protected Rectangle _box, _tickmark;
		protected TextBox _textBox;
		private readonly Texture2D _patch;
		protected IInputManager _input;
		public override bool Press => _box.Contains(_input.PointerLocation) && _input.Clicked;
		public override bool Enabled => true;
		public override BaseSwitchState State { get; protected set; }
		public Checkbox(CommonDataStruct settings, string message, Rectangle? bounds = null, bool check = false)
		{
			_input = settings.Input;
			_textBox = new(Rectangle.Empty, message, settings.Font, Color.White);
			_patch = settings.Patch;
			State = check ? BaseSwitchState.ReleasedOn : BaseSwitchState.ReleasedOff;
			if (bounds != null)
			{
				Set(bounds.Value);
			}
		}
		public void Set(Rectangle bds)
		{
			_box = new Rectangle(bds.X, bds.Y, bds.Height, bds.Height);
			_tickmark = new Rectangle(0, 0, _box.Width / 2, _box.Height / 2);
			_tickmark = DrawingExtensions.SetCenter(_box.Center, _box.Size);
			// _textBox = new(new Rectangle(_box.Right, _box.Y, bds.Width - _box.Width, _box.Height), _title, GameApp.CommonData.Font);
			_textBox.Bounds = new Rectangle(_box.Right, _box.Top, bds.Width - _box.Width, _box.Height);
		}

		public override void Draw(IBatchDrawer drawer)
		{
			drawer.Draw(_patch, _box);
			// GameApp.CommonData.Patch.Draw(_box, GameApp.CommonData.Batch, Color.White);
			if (_isChecked)
			{
				drawer.Draw(_patch, _tickmark);
			}
			// GameApp.CommonData.Patch.Draw(_tickmark, GameApp.CommonData.Batch, Color.Black);
			_textBox.Draw(drawer);
		}
	}
}