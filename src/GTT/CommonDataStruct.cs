using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct CommonDataStruct
	{
		public Rectangle ScreenBounds;
		public Color GraphDrawingBackColor;
		public Color ClearColor;
		public SpriteBatch Batch;
		public SpriteFont Font, FormalFont;
		public GameApp CurrentApp;
		public IInputManager Input;
		public Texture2D Patch, Triangle, Circle;
	}
}