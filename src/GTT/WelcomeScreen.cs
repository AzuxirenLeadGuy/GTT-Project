using System.Linq;
using System.Collections.Generic;
using Azuxiren.MG;
using Azuxiren.MG.Menu;
using Microsoft.Xna.Framework;

namespace GTT
{
	public class WelcomeScreen : IScreen
	{
		private enum State : byte { StartScreen, SelectGraphProperties, ArrangeGraph, SelectEdges, SelectAlgorithm }
		private State _state;
		private readonly Button _exitButton, _demoButton, _submitNodes, _backButton, _add_edge, _cancel_edge;
		private TextBox _welcomeText, _selectNodes, _logText, _actionlogText;
		private readonly Checkbox _directedbox, _weightedbox;
		private Rectangle _graphDragRegion;
		private readonly Picker _nodeCountPicker;
		private readonly MovableObjectManager _manager;
		private MovableObject[] _movableNodes;
		private int _nodeCount;
		private byte _nodeSel1 = 255, _nodeSel2 = 255;
		private readonly PlugKeyboard _keyboard;
		private Button[] _nodeButtons;
		private readonly Dictionary<(byte From, byte To), LineObject> _edgelines;
		private readonly Dictionary<(byte From, byte To), byte> _edgeMap;
		public WelcomeScreen()
		{
			_nodeButtons = System.Array.Empty<Button>();
			_edgelines = new();
			_edgeMap = new();
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
			y.Width += 50;
			y.Y -= 2 * x.Height + 20;
			_directedbox = new(y, "Directed");
			y.Y += x.Height + 10;
			_weightedbox = new(y, "Weighted");
			y.Y += x.Height + 10;
			y.X -= x.Width + 10;
			y.Width -= 50;
			_selectNodes = new(new(y.X - 150, y.Y - 150, y.Width + 150, y.Height + 300), "Number of nodes", GameApp.CommonData.Font);
			y.X += x.Width + 20;
			_nodeCountPicker = new Picker(Enumerable.Range(2, 19).Select(x => x.ToString()).ToArray(), y);
			y.X -= 10;
			y.Y += 2 * x.Height + 40;
			_submitNodes = new(y, "Submit");
			y.Y += x.Height + 20;
			_backButton = new Button(y, "Back");
			_graphDragRegion = new(0, 0, GameApp.CommonData.ScreenBounds.Width * 8 / 10, _submitNodes.Bounds.Top);
			_manager = new() { Region = _graphDragRegion };
			y.X = _graphDragRegion.Right;
			y.Width = GameApp.CommonData.ScreenBounds.Width - _graphDragRegion.Right;
			y.Y = GameApp.CommonData.ScreenBounds.Height - (_graphDragRegion.Height / 2);
			y.Height = _graphDragRegion.Height / 2;
			_keyboard = new PlugKeyboard(y);
			_submitNodes.Bounds.X = y.X - _submitNodes.Bounds.Width;
			_submitNodes.Set(_submitNodes.Bounds);
			_backButton.Bounds.X = _submitNodes.Bounds.X;
			_backButton.Set(_backButton.Bounds);
			y.Y /= 2;
			y.Height = y.Y / 4;
			_add_edge = new Button(y, "OK");
			y.Y += 2 * y.Height;
			_cancel_edge = new Button(y, "Cancel");
			y = new Rectangle(0, _submitNodes.Bounds.Y, _submitNodes.Bounds.X, GameApp.CommonData.ScreenBounds.Height - _graphDragRegion.Height);
			y.Height /= 2;
			_logText = new TextBox(y, "", GameApp.CommonData.Font, Color.Black);
			y.Y += y.Height;
			_actionlogText = new TextBox(y, "", GameApp.CommonData.Font, Color.Black);
		}
		public void LoadContent() { }
		public void Update(GameTime gt)
		{
			int i;
			switch (_state)
			{
				case State.StartScreen:
					_exitButton.Update(gt);
					_demoButton.Update(gt);
					_backButton.Enabled = true;
					_submitNodes.Enabled = true;
					break;
				case State.SelectGraphProperties:
					_nodeCountPicker.Update(gt);
					_directedbox.Update(gt);
					_weightedbox.Update(gt);
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
						if (_nodeButtons[i].ClickedOnUpdate(gt))
						{
							if (_nodeSel1 == 255)
								_nodeSel1 = (byte)i;
							else if (_nodeSel2 == 255)
							{
								_nodeSel2 = (byte)i;
								if (_nodeSel2 == _nodeSel1) _nodeSel2 = 255;
								else _keyboard.Value = _weightedbox.IsChecked ? 0 : 1;
							}
							else
							{
								_nodeSel1 = (byte)i;
								_nodeSel2 = 255;
							}
						}
					}
					if (_nodeSel1 == 255)
						_logText.Text = "Select the source node          ";
					else if (_nodeSel2 == 255)
						_logText.Text = "Select the destination node     ";
					else
					{
						if (_weightedbox.IsChecked)
						{
							_keyboard.Update(gt);
							_logText.Text = $"Select the weight and press OK\nto add the edge ({(char)((byte)'a' + _nodeSel1)}, {(char)((byte)'a' + _nodeSel2)})";
						}
						else
							_logText.Text = $"Select OK to add edge ({(char)((byte)'a' + _nodeSel1)}, {(char)((byte)'a' + _nodeSel2)})      ";
						if (_add_edge.ClickedOnUpdate(gt))
						{
							var key = (_nodeSel1, _nodeSel2);
							if ((_directedbox.IsChecked == false) && (key._nodeSel1 > key._nodeSel2))
								(key._nodeSel1, key._nodeSel2) = (key._nodeSel2, key._nodeSel1);
							if (_edgeMap.ContainsKey(key))
							{
								if (_weightedbox.IsChecked == false || _keyboard.Value == 0)
								{
									_edgeMap.Remove(key);
									_edgelines.Remove(key);
									_actionlogText.Text = $"Edge ({(char)((byte)'a' + _nodeSel1)}, {(char)((byte)'a' + _nodeSel2)}) is removed from the graph";
									_actionlogText.TextColor = Color.DarkGreen;
								}
								else if (_keyboard.Value < 0 || _keyboard.Value <= 255)
								{
									_edgeMap[key] = (byte)_keyboard.Value;
									if (_weightedbox.IsChecked)
									{
										_edgelines.Remove(key);
										_edgelines.Add(key, new(_movableNodes[key._nodeSel1].Bounds.Center, _movableNodes[key._nodeSel2].Bounds.Center, _directedbox.IsChecked, _keyboard.Value.ToString(), 5) { ArrowColor = Color.Yellow });
									}
									_actionlogText.Text = $"Edge is updated in the graph.\n({(char)((byte)'a' + _nodeSel1)}, {(char)((byte)'a' + _nodeSel2)}) with weight {_keyboard.Value}";
									_actionlogText.TextColor = Color.DarkGreen;
								}
								else
								{
									_actionlogText.Text = "Invalid weight. Weight should be between 1 and 255\n Enter 0 to remove existing edge";
									_actionlogText.TextColor = Color.DarkRed;
								}
							}
							else
							{
								if (_keyboard.Value == 0)
								{
									_actionlogText.Text = "Edge does not exist, and thus cannot be removed!";
									_actionlogText.TextColor = Color.DarkRed;
								}
								else if (_keyboard.Value < 0 || _keyboard.Value > 255)
								{
									_actionlogText.Text = "Invalid weight. Weight should be between 1 and 255\n Enter 0 to remove existing edge";
									_actionlogText.TextColor = Color.DarkRed;
								}
								else
								{
									_edgeMap.Add(key, (byte)_keyboard.Value);
									_edgelines.Add(key, new(_movableNodes[key._nodeSel1].Bounds.Center, _movableNodes[key._nodeSel2].Bounds.Center, _directedbox.IsChecked, _weightedbox.IsChecked ? _keyboard.Value.ToString() : null, 5) { ArrowColor = Color.Yellow });
									_actionlogText.Text = $"Edge is added to the graph.\n({(char)((byte)'a' + _nodeSel1)}, {(char)((byte)'a' + _nodeSel2)}) with weight {_keyboard.Value}";
									_actionlogText.TextColor = Color.DarkGreen;
								}
							}
							_nodeSel1 = _nodeSel2 = 255;
						}
						if (_cancel_edge.ClickedOnUpdate(gt))
							_nodeSel1 = _nodeSel2 = 255;
					}
					break;
			}

			if (_state != State.StartScreen)
			{
				if (_submitNodes.ClickedOnUpdate(gt))
				{
					byte x;
					if (_state == State.SelectGraphProperties)
					{
						_manager.CurrentlyLocked = 0;
						_manager.TotalElements = 0;
						_nodeCount = _nodeCountPicker.Index + 2;
						_movableNodes = new MovableObject[_nodeCount];
						_nodeButtons = new Button[_nodeCount];
						Rectangle pos = new(10, 10, 40, 40);
						for (x = 0; x < _nodeCount; x++)
						{
							string title = ((char)(x + 'a')).ToString();
							_movableNodes[x] = new(_manager, title, pos, Color.White);
							_nodeButtons[x] = new(pos, title);
							pos.X += 50;
							if (pos.Right >= _graphDragRegion.Width)
							{
								pos.X = 10;
								pos.Y += 50;
							}
						}
						_logText.Text = "Drag and arrange the nodes of the graph";
					}
					else if (_state == State.ArrangeGraph)
					{
						for (x = 0; x < _nodeCount; x++)
						{
							_nodeButtons?[x].Set(_movableNodes[x].Bounds);
						}
						_edgeMap.Clear();
						_edgelines.Clear();
					}
					else if (_state == State.SelectEdges)
					{
						//TODO
					}
					_state++;
				}
				if (_backButton.ClickedOnUpdate(gt))
				{
					_state--;
					_logText.Text = "";
					_actionlogText.Text = "";
					if (_state == State.StartScreen)
					{
						_backButton.Enabled = false;
						_submitNodes.Enabled = false;
					}
				}
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
					_directedbox.Draw(gt);
					_weightedbox.Draw(gt);
					_nodeCountPicker.Draw(gt);
					break;
				case State.ArrangeGraph:
					GameApp.CommonData.Batch.Draw(GameApp.CommonData.Patch, _graphDragRegion, GameApp.CommonData.GraphDrawingBackColor);
					for (i = 0; i < _nodeCount; i++)
					{
						_movableNodes[i].Draw();
					}
					_logText.Draw(GameApp.CommonData.Batch);
					break;
				case State.SelectEdges:
					GameApp.CommonData.Batch.Draw(GameApp.CommonData.Patch, _graphDragRegion, GameApp.CommonData.GraphDrawingBackColor);
					foreach (var edge in _edgelines)
					{
						edge.Value.Draw(GameApp.CommonData.Batch);
					}
					for (i = 0; i < _nodeCount; i++)
					{
						_nodeButtons[i].Draw(gt);
					}
					if (_nodeSel1 != 255 && _nodeSel2 != 255)
					{
						if (_weightedbox.IsChecked) _keyboard.Draw(gt);
						_add_edge.Draw(gt);
						_cancel_edge.Draw(gt);
					}
					_logText.Draw(GameApp.CommonData.Batch);
					_actionlogText.Draw(GameApp.CommonData.Batch);
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