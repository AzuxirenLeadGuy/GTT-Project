using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTT
{
	public struct MovableObject
	{
		public class MovableObjectManager
		{
			internal byte TotalElements = 0;
			internal byte CurrentlyLocked = 0;
			internal Rectangle Region = default;
		}
		private enum State { Unselected, Hovering, Hold }
		public readonly MovableObjectManager Manager;
		private readonly byte _id;
		private readonly Texture2D _tex;
		private readonly IInputManager _input;
		private State _state;
		public readonly Rectangle Bounds => _textBox.Bounds;
		private readonly TextBox _textBox;
		public Color Color;
		public MovableObject(
			MovableObjectManager mgr,
			string text,
			Rectangle bds,
			Texture2D tex,
			CommonDataStruct settings
		)
		{
			_tex = tex;
			_input = settings.Input;
			Manager = mgr;
			_textBox = new TextBox(
				bds,
				text,
				settings.Font,
				settings.ComponentTextColor
			);
			Color = settings.ComponentUnselectedColor;
			_id = ++Manager.TotalElements;
			_state = State.Hovering;
		}
		public void Update()
		{
			switch (_state)
			{
				case State.Unselected:
					if (Bounds.Contains(_input.PointerLocation)) _state = State.Hovering;
					break;
				case State.Hovering:
					if (!Bounds.Contains(_input.PointerLocation)) _state = State.Unselected;
					else if (_input.Clicked && Manager.CurrentlyLocked == 0)
					{
						_state = State.Hold;
						Manager.CurrentlyLocked = _id;
					}
					break;
				case State.Hold:
					if (_input.Clicked && Manager.Region.Contains(_input.PointerLocation))
					{
						Place(_input.PointerLocation);
					}
					else
					{
						_state = State.Hovering;
						Manager.CurrentlyLocked = 0;
					}
					break;
			}
		}
		public readonly void Place(Point p) => _textBox.Bounds = DrawingExtensions.SetCenter(p, Bounds.Size);
		public readonly void Draw(IBatchDrawer drawer)
		{
			drawer.Draw(_tex, destination: Bounds, color: Color);
			_textBox.Draw(drawer);
		}
	}
}