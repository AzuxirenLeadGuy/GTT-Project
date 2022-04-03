using Azuxiren.MG;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct LoadingScreen : IScreen
	{
		internal SpriteBatch Batch;
		internal Texture2D Patch;
		public void LoadContent()
		{
			Batch = GameApp.CommonData.Batch;
			Patch = GameApp.CommonData.Patch;
		}
		public void Update(GameTime gt)
		{
		}
		public void Draw(GameTime gt)
		{
		}
	}
}