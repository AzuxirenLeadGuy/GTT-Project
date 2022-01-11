using Azuxiren.MG;
using Azuxiren.MG.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GTT
{
	public class Button : AbstractButton
	{
		private readonly static Texture2D patch = GameApp.CurrentGame.patch;
		private readonly static SpriteBatch batch = GameApp.CurrentGame._spriteBatch;
		private readonly static SpriteFont font = GameApp.CurrentGame.font;
		private readonly static IInputManager input = GameApp.CurrentGame.input;
		private TextBox box;
		private Color backcolor;
		public override bool InputPressed 
		{ 
			get => bounds.Contains(input.PointerLocation) && input.Clicked; 
			set { } 
		}
		public override bool Selected 
		{ 
			get => bounds.Contains(input.PointerLocation); 
			set { } 
		}
		public Button(Rectangle bounds, string Message = "", bool EnableAtStart = true) : base(bounds, Message, EnableAtStart)
		{
			box = new TextBox(bounds, Message, font, Color.White);
		}
		public override void LoadContent() { }
		public override void Draw(GameTime gt)
		{
			switch (State)
			{
				case ComponentState.UnSelected: backcolor = Color.Black; break;
				case ComponentState.Press: backcolor = Color.White; break;
				case ComponentState.Selected: backcolor=Color.Silver; break;
				case ComponentState.Release: backcolor = Color.Silver; break;
				case ComponentState.Disabled:
					backcolor = Color.DarkRed;
					break;
			}
			batch.Draw(patch, bounds, backcolor);
			box.Draw(batch);
		}
	}
}