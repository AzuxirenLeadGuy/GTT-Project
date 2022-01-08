using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace GTT.GL
{
	public class MouseInputManager : IInputManager
	{
		public Point PointerLocation { get; set; }
		public bool Clicked { get; set; }
		public void Update()
		{
			var ms = Mouse.GetState();
			Clicked = ms.LeftButton == ButtonState.Pressed;
			PointerLocation = ms.Position;
		}
	}
}