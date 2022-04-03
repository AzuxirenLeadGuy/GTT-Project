using Azuxiren.MG;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public class Checkbox : AbstractButton, IMenuItem
	{
		protected bool _isChecked;
		protected Rectangle _box, _tickmark;
		protected TextBox _textBox;
		public override bool InputPressed
		{
			get => _box.Contains(GameApp.CommonData.Input.PointerLocation) && GameApp.CommonData.Input.Clicked;
			set { }
		}
		public override bool Selected
		{
			get => _box.Contains(GameApp.CommonData.Input.PointerLocation);
			set { }
		}
		public Checkbox(Rectangle bounds, string message = "", bool check = false, bool enableAtStart = true) : base(bounds, message, enableAtStart)
		{
			_isChecked = check;
			_box = new Rectangle(bounds.X, bounds.Y, bounds.Height, bounds.Height);
			_tickmark = new Rectangle(_box.X + 2, _box.Y + 2, _box.Width - 4, _box.Height - 4);
			_textBox = new(new Rectangle(_box.Right, _box.Y, bounds.Width - _box.Width, _box.Height), Title, GameApp.CommonData.Font);
		}
		public override void Draw(GameTime gt)
		{
			GameApp.CommonData.Patch.Draw(_box, GameApp.CommonData.Batch, Color.White);
			if (_isChecked) GameApp.CommonData.Patch.Draw(_tickmark, GameApp.CommonData.Batch, Color.Black);
			_textBox.Draw(GameApp.CommonData.Batch);
			Set(Bounds);
		}
		public void Set(Rectangle bds)
		{
			_box = new Rectangle(bds.X, bds.Y, bds.Height, bds.Height);
			_tickmark = new Rectangle(_box.X + 2, _box.Y + 2, _box.Width - 4, _box.Height - 4);
			_textBox = new(new Rectangle(_box.Right, _box.Y, bds.Width - _box.Width, _box.Height), Title, GameApp.CommonData.Font);
		}
		public override void Update(GameTime gt)
		{
			base.Update(gt);
			if (_state == ComponentState.Release) _isChecked = !_isChecked;
		}
	}
}