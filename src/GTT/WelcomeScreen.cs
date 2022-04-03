using Azuxiren.MG;
using Microsoft.Xna.Framework;

namespace GTT
{
	public struct WelcomeScreen : IScreen
	{
		private readonly Button _exitButton, _demoButton;
		internal TextBox WelcomeText;
		public WelcomeScreen()
		{
			var bound = GameApp.CommonData.GameScreen;
			WelcomeText = new(new Rectangle(0, 0, bound.Width, bound.Height / 4), "Welcome to Graph Algorithms Demo", GameApp.CommonData.FormalFont, Color.Black, Alignment.Centered);
			bound = new Rectangle(0, WelcomeText.Bounds.Bottom, bound.Width, bound.Height - WelcomeText.Bounds.Bottom);
			Rectangle x = new(0, 0, 140, 60);
			Global.SetCenter(ref x, bound);
			_exitButton = new Button(new Rectangle(x.X, x.Bottom + 20, x.Width, x.Height), "Exit"); ;
			_demoButton = new(x, "Prepare\nGraph");
			_exitButton.OnRelease += (o, e) => GameApp.CommonData.CurrentApp.Exit();
			MovableObjectManager mgr = new()
			{
				Region = new Rectangle(0, 0, 400, 400)
			};
		}
		public void LoadContent() { }
		public void Update(GameTime gt)
		{
			_exitButton.Update(gt);
			_demoButton.Update(gt);
		}
		public void Draw(GameTime gt)
		{
			WelcomeText.Draw(GameApp.CommonData.Batch);
			_demoButton.Draw(gt);
			_exitButton.Draw(gt);
		}
	}
}