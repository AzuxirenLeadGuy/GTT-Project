using Azuxiren.MG;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public class Button : AbstractButton, IMenuItem
	{
		private TextBox _box;
		private Color _backcolor;
		private readonly Texture2D _patch;
		public override bool InputPressed
		{
			get => Bounds.Contains(GameApp.CommonData.Input.PointerLocation) && GameApp.CommonData.Input.Clicked;
			set { }
		}
		public override bool Selected
		{
			get => Bounds.Contains(GameApp.CommonData.Input.PointerLocation);
			set { }
		}
		public Button(Rectangle bounds, string message = "", bool enableAtStart = true, bool circle = false) : base(bounds, message, enableAtStart)
		{
			_box = new TextBox(bounds, message, GameApp.CommonData.Font, Color.White);
			_patch = circle ? GameApp.CommonData.Circle : GameApp.CommonData.Patch;
		}
		public override void Draw(GameTime gt)
		{
			switch (State)
			{
				case ComponentState.UnSelected: _backcolor = Color.Black; break;
				case ComponentState.Press: _backcolor = Color.White; break;
				case ComponentState.Selected: _backcolor = Color.Silver; break;
				case ComponentState.Release: _backcolor = Color.Silver; break;
				case ComponentState.Disabled:
					_backcolor = Color.DarkRed;
					break;
			}
			GameApp.CommonData.Batch.Draw(_patch, Bounds, _backcolor);
			_box.Draw(GameApp.CommonData.Batch);
		}
		public void Set(Rectangle bds)
		{
			Bounds = bds;
			_box.Bounds = bds;
		}
		public bool ClickedOnUpdate(GameTime gt)
		{
			if (_state != ComponentState.Selected)
			{
				Update(gt);
				return false;
			}
			else
			{
				Update(gt);
				return _state == ComponentState.Press;
			}
		}
	}
}