using System.Linq;
using System.Collections.Generic;
using Azuxiren.MG;
using Microsoft.Xna.Framework;

namespace GTT
{
	public class WelcomeScreen : IScreen
	{
		private enum State : byte { StartScreen, SelectGraphProperties, ArrangeGraph, SelectEdges, SelectAlgorithm, NormalShowcase, FloydShowcase, }
		private State _state;
		private readonly Button _exitButton, _demoButton, _submitNodes, _backButton, _add_edge, _cancel_edge, _next_update, _reset_update;
		private TextBox _welcomeText, _selectNodes, _logText, _actionlogText, _selectAlgo;
		private readonly Checkbox _directedbox, _weightedbox;
		private Rectangle _graphDragRegion;
		private readonly Picker _nodeCountPicker;
		private Picker _algoPicker, _sourcePicker, _destPicker;
		private readonly MovableObjectManager _manager;
		private MovableObject[] _movableNodes;
		private int _nodeCount;
		private byte _nodeSel1 = 255, _nodeSel2 = 255;
		private readonly PlugKeyboard _keyboard;
		private Button[] _nodeButtons;
		private readonly Dictionary<(byte From, byte To), LineObject> _edgelines;
		private byte[,] _edgeMap;
		private GraphDrawing _commonGraphShowcase;
		private GraphDrawingUpdate[] _commonGraphUpdates;
		private int _currentUpdateIndex, _lastUpdateIndex;
		public WelcomeScreen()
		{
			_movableNodes = null!;
			_edgeMap = null!;
			_algoPicker = null!;
			_sourcePicker = null!;
			_destPicker = null!;
			_commonGraphShowcase = default;
			_commonGraphUpdates = null!;
			_currentUpdateIndex = _lastUpdateIndex = 0;
			_nodeButtons = System.Array.Empty<Button>();
			_edgelines = new();
			_state = State.StartScreen;
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
			_add_edge = new(y, "OK");
			_next_update = new(y, "Next");
			y.Y += 2 * y.Height;
			_cancel_edge = new(y, "Cancel");
			_reset_update = new(y, "Reset");
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
						string keytext = $"({GameApp.GetLabel(_nodeSel1)},{GameApp.GetLabel(_nodeSel2)})";
						if (_weightedbox.IsChecked)
						{
							_keyboard.Update(gt);
							_logText.Text = $"Select the weight and press OK\nto add the edge {keytext}";
						}
						else
							_logText.Text = $"Select OK to add edge {keytext}      ";
						if (_add_edge.ClickedOnUpdate(gt))
						{
							var key = (_nodeSel1, _nodeSel2);
							keytext = $"({GameApp.GetLabel(key._nodeSel1)},{GameApp.GetLabel(key._nodeSel2)})";
							if ((_directedbox.IsChecked == false) && (key._nodeSel1 > key._nodeSel2))
								(key._nodeSel1, key._nodeSel2) = (key._nodeSel2, key._nodeSel1);
							if (_edgeMap[_nodeSel1, _nodeSel2] != 0)
							{
								if (_weightedbox.IsChecked == false || _keyboard.Value == 0)
								{
									_edgeMap[_nodeSel1, _nodeSel2] = 0;
									if (_directedbox.IsChecked == false)
										_edgeMap[_nodeSel2, _nodeSel1] = 0;
									_edgelines.Remove(key);
									_actionlogText.Text = $"Edge {keytext} is removed from the graph";
									_actionlogText.TextColor = Color.DarkGreen;
								}
								else if (_keyboard.Value < 0 || _keyboard.Value <= 255)
								{
									_edgeMap[_nodeSel1, _nodeSel2] = (byte)_keyboard.Value;
									if (_directedbox.IsChecked == false)
										_edgeMap[_nodeSel2, _nodeSel1] = _edgeMap[_nodeSel1, _nodeSel2];
									if (_weightedbox.IsChecked)
									{
										_edgelines.Remove(key);
										_edgelines.Add(key, CreateLine(key));
									}
									_actionlogText.Text = $"Edge is updated in the graph.\n{keytext} with weight {_keyboard.Value}";
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
									_edgeMap[_nodeSel1, _nodeSel2] = (byte)_keyboard.Value;
									if (_directedbox.IsChecked == false)
										_edgeMap[_nodeSel2, _nodeSel1] = _edgeMap[_nodeSel1, _nodeSel2];
									_edgelines.Add(key, CreateLine(key));
									_actionlogText.Text = $"Edge is added to the graph.\n({GameApp.GetLabel(key._nodeSel1)}, {GameApp.GetLabel(key._nodeSel1)}) with weight {_keyboard.Value}";
									_actionlogText.TextColor = Color.DarkGreen;
								}
							}
							_nodeSel1 = _nodeSel2 = 255;
						}
						if (_cancel_edge.ClickedOnUpdate(gt))
							_nodeSel1 = _nodeSel2 = 255;
					}
					break;
				case State.SelectAlgorithm:
					_algoPicker.Update(gt);
					_sourcePicker.Update(gt);
					_destPicker.Update(gt);
					break;
				case State.NormalShowcase:
					if (_next_update.ClickedOnUpdate(gt))
					{
						_logText.Text = _commonGraphUpdates[_currentUpdateIndex].UpdateLog;
						_commonGraphShowcase.Update(_commonGraphUpdates[_currentUpdateIndex++]);
						if (_currentUpdateIndex == _lastUpdateIndex) _next_update.Enabled = false;
					}
					if (_reset_update.ClickedOnUpdate(gt))
						ResetAlgo();
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
							string title = GameApp.GetLabel(x);
							_movableNodes[x] = new(_manager, title, pos, Color.White);
							_nodeButtons[x] = new Button(pos, title, circle: true);
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
						_edgeMap = new byte[_nodeCount, _nodeCount];
						_edgelines.Clear();
					}
					else if (_state == State.SelectEdges)
					{
						Rectangle algobd = new(0, 0, _graphDragRegion.Width / 2, 80);
						Global.SetCenter(ref algobd, GameApp.CommonData.ScreenBounds);
						string[] options = _directedbox.IsChecked
							? (new string[] { "Depth First Search", "Breadth First Search", "Dijkstra Algorithm", "Floyd-Warshall Algorithm" })
							: (new string[] { "Depth First Search", "Breadth First Search", "Dijkstra Algorithm", "Floyd-Warshall Algorithm", "Kruskal's Algorithm" });
						_algoPicker = new(options, algobd);
						_commonGraphShowcase = new(_movableNodes.Select(x => x.Bounds).ToArray(), _edgelines);
						algobd.Y -= 2 * algobd.Height;
						_selectAlgo = new TextBox(algobd, "Select the algorithm", GameApp.CommonData.Font);
						string[] sx = new string[_nodeCount], dx = new string[_nodeCount + 1];
						for (byte j = 0; j < _nodeCount; j++)
						{
							sx[j] = dx[j] = GameApp.GetLabel(j);
						}
						dx[_nodeCount] = "None";
						algobd.Y -= 2 * algobd.Height;
						algobd.Width /= 3;
						_sourcePicker = new(sx, algobd);
						algobd.X += 2 * algobd.Width;
						_destPicker = new(dx, algobd);
					}
					else if (_state == State.SelectAlgorithm)
					{
						switch (_algoPicker.Index)
						{
							case 0:
								_commonGraphUpdates = Algorithms.DepthFirstSearch((byte)_nodeCount, _edgeMap, _sourcePicker.Index, _destPicker.Index).ToArray();
								goto default;
							case 1:
								_commonGraphUpdates = Algorithms.BreadthFirstSearch((byte)_nodeCount, _edgeMap, _sourcePicker.Index, _destPicker.Index).ToArray();
								goto default;
							case 2:
								_commonGraphUpdates = Algorithms.Dijkstra((byte)_nodeCount, _edgeMap, _sourcePicker.Index, _destPicker.Index).ToArray();
								goto default;
							case 4:
								_commonGraphUpdates = Algorithms.Kruskal((byte)_nodeCount, _edgeMap).ToArray();
								goto default;
							case 3://TODO
								_state++;
								break;
							default:
								ResetAlgo();
								break;
						}
					}
					_state++;
				}
				if (_backButton.ClickedOnUpdate(gt))
				{
					if (_state == State.FloydShowcase) _state--;
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
				case State.SelectAlgorithm:
					_selectAlgo.Draw(GameApp.CommonData.Batch);
					_algoPicker.Draw(gt);
					if (_algoPicker.Index < 3)
					{
						_sourcePicker.Draw(gt);
						_destPicker.Draw(gt);
					}
					break;
				case State.NormalShowcase:
					GameApp.CommonData.Batch.Draw(GameApp.CommonData.Patch, _graphDragRegion, GameApp.CommonData.GraphDrawingBackColor);
					_commonGraphShowcase.Draw();
					_next_update.Draw(gt);
					_reset_update.Draw(gt);
					_logText.Draw(GameApp.CommonData.Batch);
					break;
			}
			if (_state != State.StartScreen)
			{
				_backButton.Draw(gt);
				if (_state < State.NormalShowcase)
					_submitNodes.Draw(gt);
			}
		}
		private void ResetAlgo()
		{
			_commonGraphShowcase.Reset();
			_lastUpdateIndex = _commonGraphUpdates.Length;
			_currentUpdateIndex = 1;
			_logText.Text = _commonGraphUpdates[0].UpdateLog;
			_commonGraphShowcase.Update(_commonGraphUpdates[0]);
			_next_update.Enabled = true;
		}
		private LineObject CreateLine((byte from, byte to) key)
		{
			LineObject x = new(_movableNodes[key.from].Bounds.Center, _movableNodes[key.to].Bounds.Center, _directedbox.IsChecked, _keyboard.Value.ToString(), 5) { ArrowColor = Color.Yellow };
			x.SetLabelColor(Color.White);
			return x;
		}
	}
}