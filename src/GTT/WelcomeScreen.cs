using System.Linq;

using Azuxiren.MG;

using Microsoft.Xna.Framework;

namespace GTT
{
	public class WelcomeScreen : IScreen
	{
		private enum State : byte { StartScreen, SelectGraphProperties, ArrangeGraph, SelectEdges, SelectAlgorithm }
		private State _state;
		private readonly Button _exitButton, _demoButton, _submitNodes, _backButton, _add_edge, _cancel_edge;
		private readonly TextBox _welcomeText, _selectNodes, _logText;
		private readonly Checkbox _directedbox, _weightedbox;
		private Rectangle _graphDragRegion;
		private readonly Picker _nodeCountPicker;
		private readonly MovableObjectManager _manager;
		private MovableObject[] _movableNodes;
		private int _nodeCount;
		private byte _nodeSel1 = 255, _nodeSel2 = 255;
		private readonly PlugKeyboard _keyboard;
		private Button[] _nodeButtons;
		public WelcomeScreen()
		{
			_state = State.StartScreen;
			_movableNodes = null!;
			var bound = GameApp.CommonData.ScreenBounds;
			_welcomeText = new(new Rectangle(0, 0, bound.Width, bound.Height / 4), "Welcome to Graph Algorithms Demo", GameApp.CommonData.FormalFont, Color.Black, Alignment.Centered);
			bound = new Rectangle(0, _welcomeText.Bounds.Bottom, bound.Width, bound.Height - _welcomeText.Bounds.Bottom);
			Rectangle x = new(0, 0, 140, 60);
			Global.SetCenter(ref x, bound);
			_exitButton = new Button(new Rectangle(x.X, x.Bottom + 20, x.Width, x.Height), "Exit"); ;
			_exitButton.OnRelease += (o, e) => GameApp.CommonData.CurrentApp.Exit();
			_demoButton = new(x, "Prepare\nGraph");
			_demoButton.OnRelease += (o, e) => _state = State.SelectGraphProperties;
			Rectangle y = x;
			y.Y -= 2 * x.Height + 20;
			_directedbox = new(y, "Directed");
			y.Y += x.Height + 10;
			_weightedbox = new(y, "Weighted");
			y.Y += x.Height + 10;
			y.X -= x.Width + 10;
			_selectNodes = new(new(y.X - 100, y.Y - 100, y.Width + 100, y.Height + 100), "Number of nodes", GameApp.CommonData.Font);
			y.X += x.Width + 20;
			_nodeCountPicker = new Picker(Enumerable.Range(2, 19).Select(x => x.ToString()).ToArray(), y);
			y.X -= 10;
			y.Y += 2 * x.Height + 40;
			_submitNodes = new(y, "Submit");
			y.Y += x.Height + 20;
			_backButton = new Button(y, "Back");
			_backButton.OnRelease += (o, e) => _state--;
			_graphDragRegion = new(0, 0, GameApp.CommonData.ScreenBounds.Width * 8 / 10, _submitNodes.Bounds.Top);
			_manager = new() { Region = _graphDragRegion };
			_keyboard = new PlugKeyboard(new Rectangle(_graphDragRegion.Right, _graphDragRegion.Y, GameApp.CommonData.ScreenBounds.Right - _graphDragRegion.Right, _graphDragRegion.Height / 2));
			_submitNodes.OnRelease += (o, e) =>
			{
				byte i;
				if (_state == State.SelectGraphProperties)
				{
					_manager.CurrentlyLocked = 0;
					_manager.TotalElements = 0;
					_nodeCount = _nodeCountPicker.Index + 2;
					_movableNodes = new MovableObject[_nodeCount];
					_nodeButtons = new Button[_nodeCount];
					for (i = 0; i < _nodeCount; i++)
					{
						string title = ((char)(i + 'a')).ToString();
						_movableNodes[i] = new(_manager, title, new Rectangle(10, 10, 40, 40), Color.White);
						_nodeButtons[i] = new(new Rectangle(10, 10, 40, 40), title);
					}
				}
				else if (_state == State.ArrangeGraph)
				{
					for (i = 0; i < _nodeCount; i++)
					{
						_nodeButtons?[i].Set(_movableNodes[i].Bounds);
					}
				}
				else if (_state == State.SelectEdges)
				{

				}
				_state++;
			};
		}
		public void LoadContent() { }
		public void Update(GameTime gt)
		{
			int i;
			if (_state != State.StartScreen)
			{
				_backButton.Update(gt);
				_submitNodes.Update(gt);
			}
			switch (_state)
			{
				case State.StartScreen:
					_exitButton.Update(gt);
					_demoButton.Update(gt);
					break;
				case State.SelectGraphProperties:
					_nodeCountPicker.Update(gt);
					break;
				case State.ArrangeGraph:
					for (i = _nodeCount - 1; i >= 0; i--)
					{
						_movableNodes[i].Update();
					}
					break;
				case State.SelectEdges:
					for (i = 0; i < _nodeCount; i++)
					{
						if (_nodeButtons[i].State == Azuxiren.MG.Menu.ComponentState.Release)
						{
							if (_nodeSel1 == 255)
								_nodeSel1 = (byte)i;
							else if (_nodeSel2 == 255)
								_nodeSel2 = (byte)i;
							else
							{
								_nodeSel1 = (byte)i;
								_nodeSel2 = 255;
							}
						}
					}
					if (_nodeSel1 != 255 && _nodeSel2 != 255)
					{
						_keyboard.Update(gt);
						_add_edge.Update(gt);
						_cancel_edge.Update(gt);
					}
					break;
			}
		}
		public void Draw(GameTime gt)
		{
			byte i;
			switch (_state)
			{
				case State.StartScreen:
					_welcomeText.Draw(GameApp.CommonData.Batch);
					_demoButton.Draw(gt);
					_exitButton.Draw(gt);
					break;
				case State.SelectGraphProperties:
					_selectNodes.Draw(GameApp.CommonData.Batch);
					_backButton.Draw(gt);
					_nodeCountPicker.Draw(gt);
					break;
				case State.ArrangeGraph:
					GameApp.CommonData.Batch.Draw(GameApp.CommonData.Patch, _graphDragRegion, GameApp.CommonData.GraphDrawingBackColor);
					for (i = 0; i < _nodeCount; i++)
					{
						_movableNodes[i].Draw();
					}
					break;
				case State.SelectEdges:
					GameApp.CommonData.Batch.Draw(GameApp.CommonData.Patch, _graphDragRegion, GameApp.CommonData.GraphDrawingBackColor);
					for(i=0;i<_nodeCount;i++)
					{
						_nodeButtons[i].Draw(gt);
					}
					if (_nodeSel1 != 255 && _nodeSel2 != 255)
					{
						_keyboard.Draw(gt);
						_add_edge.Draw(gt);
						_cancel_edge.Draw(gt);
					}
					_logText.Draw(GameApp.CommonData.Batch);
					break;
			}
			if (_state != State.StartScreen)
			{
				_backButton.Draw(gt);
				_submitNodes.Draw(gt);
			}
		}
	}
}