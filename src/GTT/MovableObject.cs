using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public class MovableObjectManager
	{
		internal byte TotalElements = 0;
		internal byte CurrentlyLocked = 0;
		internal Rectangle Region;
	}
	public struct MovableObject
	{
		private enum State { Unselected, Selected, Hold }
		private readonly static Texture2D Patch = GameApp.CommonData.Patch;
		private readonly static SpriteBatch Batch = GameApp.CommonData.Batch;
		private readonly static IInputManager Input = GameApp.CommonData.Input;
		public readonly MovableObjectManager Manager;
		private readonly byte _id;
		private State _state;
		private Rectangle _box;
		public Color Color;
		public MovableObject(MovableObjectManager mgr, Rectangle bds, Color color)
		{
			Manager = mgr;
			_box = bds;
			Color = color;
			_id = ++Manager.TotalElements;
			_state = State.Selected;
		}
		public void Update()
		{
			switch (_state)
			{
				case State.Unselected:
					if (_box.Contains(Input.PointerLocation)) _state = State.Selected;
					break;
				case State.Selected:
					if (!_box.Contains(Input.PointerLocation)) _state = State.Unselected;
					else if (Input.Clicked && Manager.CurrentlyLocked == 0)
					{
						_state = State.Hold;
						Manager.CurrentlyLocked = _id;
					}
					break;
				case State.Hold:
					if (Input.Clicked && Manager.Region.Contains(Input.PointerLocation))
						Place(Input.PointerLocation);
					else
					{
						_state = State.Selected;
						Manager.CurrentlyLocked = 0;
					}
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
			Batch.Draw(Patch, _box, Color);
		}
	}
}