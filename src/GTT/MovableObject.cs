using Azuxiren.MG;

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
		public readonly MovableObjectManager Manager;
		private readonly byte _id;
		private State _state;
		private Rectangle _box;
		private TextBox _textBox;
		public Color Color;
		public MovableObject(MovableObjectManager mgr, string text, Rectangle bds, Color color)
		{
			Manager = mgr;
			_box = bds;
			_textBox = new TextBox(bds, text, GameApp.CommonData.Font, Color.Black);
			Color = color;
			_id = ++Manager.TotalElements;
			_state = State.Selected;
		}
		public void Update()
		{
			switch (_state)
			{
				case State.Unselected:
					if (_box.Contains(GameApp.CommonData.Input.PointerLocation)) _state = State.Selected;
					break;
				case State.Selected:
					if (!_box.Contains(GameApp.CommonData.Input.PointerLocation)) _state = State.Unselected;
					else if (GameApp.CommonData.Input.Clicked && Manager.CurrentlyLocked == 0)
					{
						_state = State.Hold;
						Manager.CurrentlyLocked = _id;
					}
					break;
				case State.Hold:
					if (GameApp.CommonData.Input.Clicked && Manager.Region.Contains(GameApp.CommonData.Input.PointerLocation))
					{
						Place(GameApp.CommonData.Input.PointerLocation);
					}
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
			Global.SetCenter(ref _box, p);
			_textBox.Bounds = _box;
		}
		public void Draw()
		{
			GameApp.CommonData.Batch.Draw(GameApp.CommonData.Patch, _box, Color);
			_textBox.Draw(GameApp.CommonData.Batch);
		}
	}
}