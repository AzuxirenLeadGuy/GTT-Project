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
		internal MovableObject o1, o2, o3;
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
			o1.LoadContent();
			o2.LoadContent();
			o2.Place(new Point(0, 100));
			o2._color = Color.Blue;
			o3.LoadContent();
			o3.Place(new Point(100, 0));
			o3._color = Color.Green;
		}
		public void Update(GameTime gt)
		{
			o1.Update();
			o2.Update();
			o3.Update();
			_exitButton.Update(gt);
		}
		public void Draw(GameTime gt)
		{
			o3.Draw();
			o2.Draw();
			o1.Draw();
			_exitButton.Draw(gt);
		}
	}
}