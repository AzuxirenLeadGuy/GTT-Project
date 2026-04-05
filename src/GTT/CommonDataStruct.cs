using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct CommonDataStruct
	{
		// public Rectangle ScreenBounds;
		public Color GraphDrawingBackColor;
		public Color ClearColor;
		public SpriteFont Font, FormalFont;
		public IInputManager Input;
		public Texture2D Patch, Triangle, Circle;
		public Color ComponentTextColor;
		public Color[] ComponentPalette;
		public int GameWidth, GameHeight;
	}
}