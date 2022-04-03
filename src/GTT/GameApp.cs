using Azuxiren.MG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GTT
{
	public class GameApp : AMGC<WelcomeScreen, LoadingScreen>
	{
		internal static CommonDataStruct CommonData = new();
		public GameApp(IInputManager inputManager)
		{
			Content.RootDirectory = "Content";
			CommonData.ClearColor = Color.DarkGray;
			IsMouseVisible = true;
			CommonData.Input = inputManager;
		}
		protected override void LoadContent()
		{
			CommonData.Font = Content.Load<SpriteFont>("font");
			CommonData.FormalFont = Content.Load<SpriteFont>("ffont");
			CommonData.Batch = new SpriteBatch(GraphicsDevice);
			CommonData.Patch = new Texture2D(GraphicsDevice, 1, 1);
			CommonData.Patch.SetData(new Color[] { Color.White });
			CommonData.Triangle = Content.Load<Texture2D>("triangle");
			CommonData.Circle = Content.Load<Texture2D>("circle");
			SetFullScreen();
			base.LoadContent();
		}
		protected override void Draw(GameTime gt)
		{
			GraphicsDevice.Clear(CommonData.ClearColor);
			CommonData.Batch.Begin();
			base.Draw(gt);
			CommonData.Batch.End();
		}
		protected override void Update(GameTime gameTime)
		{
			CommonData.Input.Update();
			base.Update(gameTime);
		}
	}
}