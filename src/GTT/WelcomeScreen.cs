using System.Collections.Generic;

using Azuxiren.MG;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct WelcomeScreen : IScreen
	{
		private readonly Button _exitButton, _demoButton;
		internal TextBox WelcomeText;
		private readonly Checkbox _cbox1, _cbox2;
		public WelcomeScreen()
		{
			var bound = GameApp.CommonData.GameScreen;
			WelcomeText = new(new Rectangle(0, 0, bound.Width, bound.Height * 1 / 4), "Welcome to Graph Algorithms Demo", GameApp.CommonData.FormalFont);
			bound = new Rectangle(0, 0, bound.Width, bound.Height);
			var x = new Rectangle(0, 0, 140, 60);
			_exitButton = new Button(x, "Exit");
			_exitButton.OnRelease += (o, e) => GameApp.CommonData.CurrentApp.Exit();
			Global.SetCenter(ref x, bound);
			MovableObjectManager mgr = new()
			{
				Region = new Rectangle(0, 0, 400, 400)
			};
			_cbox1 = new(new Rectangle(bound.Width / 2, WelcomeText.Bounds.Bottom, 300, 50), "Undirected", true);
			_cbox2 = new(new Rectangle(bound.Width / 2, _cbox1.Bounds.Bottom + 20, 300, 50), "UnWeighted", false);
			_demoButton = new(new Rectangle(), "");
		}
		public void LoadContent() { }
		public void Update(GameTime gt)
		{
			_exitButton.Update(gt);
			_demoButton.Update(gt);
			_cbox1.Update(gt);
			_cbox2.Update(gt);
		}
		public void Draw(GameTime gt)
		{
			WelcomeText.Draw(GameApp.CommonData.Batch);
			_demoButton.Draw(gt);
			_exitButton.Draw(gt);
			_cbox1.Draw(gt);
			_cbox2.Draw(gt);
		}
	}
}