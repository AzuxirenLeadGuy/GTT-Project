using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct MovableObject
	{
		private enum State { Unselected, Selected, Hold }
		private readonly static Texture2D patch = GameApp.CurrentGame.patch;
		private readonly static SpriteBatch batch = GameApp.CurrentGame._spriteBatch;
		private readonly static IInputManager input = GameApp.CurrentGame.input;
		internal static byte TotalElements = 0;
		internal static byte CurrentlyLocked = 0;
		private byte _id;
		private State _state;
		private Rectangle _box;
		internal Color _color;
		public void LoadContent()
		{
			_box = new Rectangle(20, 20, 20, 20);
			_color = Color.White;
			_state = State.Unselected;
			_id = ++TotalElements;
		}
		public void Update()
		{
			switch (_state)
			{
				case State.Unselected:
					if (_box.Contains(input.PointerLocation)) _state = State.Selected;
					break;
				case State.Selected:
					if (!_box.Contains(input.PointerLocation)) _state = State.Unselected;
					else if (input.Clicked && CurrentlyLocked == 0)
					{
						_state = State.Hold;
						CurrentlyLocked = _id;
					}
					break;
				case State.Hold:
					if (!input.Clicked)
					{
						_state = State.Selected;
						CurrentlyLocked = 0;
					}
					else Place(input.PointerLocation);
					break;
			}
		}
		public void Place(Point p)
		{
			_box.X = p.X;
			_box.Y = p.Y;
		}
		public void Draw()
		{
			batch.Draw(patch, _box, _color);
		}
	}
}