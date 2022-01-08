using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Azuxiren.MG;
namespace GTT
{
	public class GameApp : AMGC<WelcomeScreen, LoadingScreen>
	{
		internal static GameApp CurrentGame;
		internal Color ClearColor = Color.DarkGray;
		internal SpriteBatch _spriteBatch;
		internal Texture2D patch, triangle, circle;
		internal SpriteFont font;
		internal IInputManager input;
		public GameApp(IInputManager inputManager)
		{
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			input = inputManager;
			CurrentGame = this;
		}
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			patch = new Texture2D(GraphicsDevice, 1, 1);
			patch.SetData(new Color[] { Color.White });
			triangle = Content.Load<Texture2D>("triangle");
			circle = Content.Load<Texture2D>("circle");
			font = Content.Load<SpriteFont>("font");
			SetFullScreen();
			base.LoadContent();
		}
		protected override void Draw(GameTime gt)
		{
			GraphicsDevice.Clear(ClearColor);
			_spriteBatch.Begin();
			base.Draw(gt);
			_spriteBatch.End();
		}
		protected override void Update(GameTime gameTime)
		{
			input.Update();
			base.Update(gameTime);
		}
	}
}
