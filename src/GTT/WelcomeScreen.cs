using System.Collections.Generic;

using Azuxiren.MG;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct WelcomeScreen : IScreen
	{
		enum State : byte { Welcome, SelectAlgo, EnterGraph };
		private State _state;
		private Button _exitButton;
		internal readonly SpriteBatch Batch = GameApp.CurrentGame.SpriteBatch;
		internal readonly Texture2D Patch = GameApp.CurrentGame.Patch;
		internal MovableObject O1, O2, O3;
		public void LoadContent()
		{
			var bound = GameApp.CurrentGame.Window.ClientBounds;
			_state = State.Welcome;
			bound = new Rectangle(0, 0, bound.Width, bound.Height);
			var x = new Rectangle(0, 0, 140, 60);
			Global.SetCenter(ref x, bound);
			_exitButton = new Button(x, "Exit");
			_exitButton.OnRelease += (o, e) => GameApp.CurrentGame.Exit();
			MovableObjectManager mgr = new()
			{
				Region = new Rectangle(0, 0, 400, 400)
			};
			O1 = new MovableObject(mgr, new Rectangle(0, 0, 20, 20), Color.Red);
			O2 = new MovableObject(mgr, new Rectangle(0, 0, 20, 20), Color.Blue);
			O3 = new MovableObject(mgr, new Rectangle(0, 0, 20, 20), Color.Green);
		}
		public void Update(GameTime gt)
		{
			O1.Update();
			O2.Update();
			O3.Update();
			_exitButton.Update(gt);
		}
		public void Draw(GameTime gt)
		{
			O3.Draw();
			O2.Draw();
			O1.Draw();
			_exitButton.Draw(gt);
		}
	}
}