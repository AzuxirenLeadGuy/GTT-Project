using Azuxiren.MG;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Azuxiren.MG.Menu;

namespace GTT
{
	public struct WelcomeScreen : IScreen
	{
		enum State : byte { Welcome, SelectAlgo, EnterGraph };
		State state;
		private Button _exitButton;
		internal SpriteBatch batch;
		internal Texture2D patch;
		public void LoadContent()
		{
			batch = GameApp.CurrentGame._spriteBatch;
			patch = GameApp.CurrentGame.patch;
			var bound = GameApp.CurrentGame.Window.ClientBounds;
			state = State.Welcome;
			bound = new Rectangle(0, 0, bound.Width, bound.Height);
			var x = new Rectangle(0, 0, 140, 60);
			Global.SetCenter(ref x, bound);
			_exitButton = new Button(x, "Exit");
			_exitButton.OnRelease += (o, e) => GameApp.CurrentGame.Exit();
		}
		public void Update(GameTime gt)
		{
			_exitButton.Update(gt);
		}
		public void Draw(GameTime gt)
		{
			_exitButton.Draw(gt);
		}
	}
}