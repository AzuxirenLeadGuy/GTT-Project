using Azuxiren.MG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GTT
{
	public class GameApp : AMGC<WelcomeScreen, LoadingScreen>
	{
		internal static GameApp CurrentGame;
		internal Color ClearColor = Color.DarkGray;
		internal SpriteBatch SpriteBatch;
		internal Texture2D Patch, Triangle, Circle;
		internal SpriteFont Font;
		internal IInputManager Input;
		internal int UnitLength;
		public GameApp(IInputManager inputManager)
		{
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Input = inputManager;
			CurrentGame = this;
		}
		protected override void LoadContent()
		{
			UnitLength = Window.ClientBounds.Height >> 5;
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			Patch = new Texture2D(GraphicsDevice, 1, 1);
			Patch.SetData(new Color[] { Color.White });
			Triangle = Content.Load<Texture2D>("triangle");
			Circle = Content.Load<Texture2D>("circle");
			Font = Content.Load<SpriteFont>("font");
			SetFullScreen();
			base.LoadContent();
		}
		protected override void Draw(GameTime gt)
		{
			GraphicsDevice.Clear(ClearColor);
			SpriteBatch.Begin();
			base.Draw(gt);
			SpriteBatch.End();
		}
		protected override void Update(GameTime gameTime)
		{
			Input.Update();
			base.Update(gameTime);
		}
	}
}