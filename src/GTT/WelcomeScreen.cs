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
		Button button;
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
			button = new Button(x, "Press me!");
			button.OnRelease += OnButtonPress;
		}
		public void OnButtonPress(object o, ComponentArgs e)
		{
			if(e.Current != ComponentState.Release) return;
			var x = GameApp.CurrentGame.ClearColor;
			x = x == Color.DarkGray ? Color.White : Color.DarkGray;
			GameApp.CurrentGame.ClearColor = x;
		}
		public void Update(GameTime gt)
		{
			button.Update(gt);
		}
		public void Draw(GameTime gt)
		{
			button.Draw(gt);
		}
	}
}