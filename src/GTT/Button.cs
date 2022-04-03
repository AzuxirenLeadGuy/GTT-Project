using Azuxiren.MG;
using Azuxiren.MG.Menu;
using Microsoft.Xna.Framework;
namespace GTT
{
	public class Button : AbstractButton, IMenuItem
	{
		private TextBox _box;
		private Color _backcolor;
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
		public Button(Rectangle bounds, string message = "", bool enableAtStart = true) : base(bounds, message, enableAtStart)
		{
			_box = new TextBox(bounds, message, GameApp.CommonData.Font, Color.White);
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
			GameApp.CommonData.Batch.Draw(GameApp.CommonData.Patch, Bounds, _backcolor);
			_box.Draw(GameApp.CommonData.Batch);
		}
		public void Set(Rectangle bds)
		{
			Bounds = bds;
			_box.Bounds = bds;
		}
	}
}