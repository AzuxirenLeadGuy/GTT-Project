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
			CommonData.ClearColor = new Color(0xc5, 0xca, 0xe9);
			IsMouseVisible = true;
			CommonData.Input = inputManager;
		}
		protected override void LoadContent()
		{
			SetFullScreen();
			CommonData.Font = Content.Load<SpriteFont>("font");
			CommonData.FormalFont = Content.Load<SpriteFont>("ffont");
			CommonData.Batch = new SpriteBatch(GraphicsDevice);
			CommonData.Patch = new Texture2D(GraphicsDevice, 1, 1);
			CommonData.Patch.SetData(new Color[] { Color.White });
			CommonData.Triangle = Content.Load<Texture2D>("triangle");
			CommonData.Circle = Content.Load<Texture2D>("circle");
			Rectangle screen = Window.ClientBounds;
			CommonData.CurrentApp = this;
			CommonData.GraphDrawingBackColor = new Color(0x30, 0x3f, 0x9f);
			CommonData.ScreenBounds = new(0, 0, screen.Width, screen.Height);
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
		public static string GetLabel(byte x) => ((char)((byte)'A' + x)).ToString();
	}
}