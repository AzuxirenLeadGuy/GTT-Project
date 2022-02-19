using Azuxiren.MG;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GTT
{
	public class Button : AbstractButton, IMenuItem
	{
		private readonly static Texture2D Patch = GameApp.CurrentGame.Patch;
		private readonly static SpriteBatch Batch = GameApp.CurrentGame.SpriteBatch;
		private readonly static IInputManager Input = GameApp.CurrentGame.Input;
		private readonly static SpriteFont Font = GameApp.CurrentGame.Font;
		private TextBox _box;
		private Color _backcolor;
		public override bool InputPressed
		{
			get => Bounds.Contains(Input.PointerLocation) && Input.Clicked;
			set { }
		}
		public override bool Selected
		{
			get => Bounds.Contains(Input.PointerLocation);
			set { }
		}
		public Button(Rectangle bounds, string message = "", bool enableAtStart = true) : base(bounds, message, enableAtStart)
		{
			_box = new TextBox(bounds, message, Font, Color.White);
		}
		public override void LoadContent() { }
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
			Batch.Draw(Patch, Bounds, _backcolor);
			_box.Draw(Batch);
		}
		public void Set(Rectangle bds)
		{
			Bounds = bds;
			_box.Bounds = bds;
		}
	}
}