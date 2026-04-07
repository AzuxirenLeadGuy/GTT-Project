using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct CommonDataStruct
	{
		public SpriteFont Font, FormalFont;
		public IInputManager Input;
		public Texture2D Patch, Triangle, Circle;
		public Color ClearColor;
		public Color ComponentTextColor;
		public Color ComponentUnselectedColor;
		public Color ComponentHoverColor;
		public Color ComponentPressColor;
		public Color ComponentDisabledColor;
		public Color[] ComponentPalette;
		public Color[] GdColors;
		public Color GDBaseColor;
		public Color GDNodeColor;
		public Color GDArrowColor;
		public int GameWidth, GameHeight;
	}
}