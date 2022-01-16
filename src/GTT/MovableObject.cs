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
		private State _state;
		private Rectangle _box;
		internal Color _color;
		public void LoadContent()
		{
			_box = new Rectangle(20, 20, 20, 20);
			_color = Color.White;
			_state = State.Unselected;
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
					else if (input.Clicked) _state = State.Hold;
					break;
				case State.Hold:
					if (!input.Clicked) _state = State.Selected;
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