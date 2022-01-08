using Azuxiren.MG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct LoadingScreen : IScreen
	{
		internal SpriteBatch batch;
		internal Texture2D patch;
		public void LoadContent()
		{
			batch = GameApp.CurrentGame._spriteBatch;
			patch = GameApp.CurrentGame.patch;
		}
		public void Update(GameTime gt)
		{
		}
		public void Draw(GameTime gt)
		{
		}
	}
}