using System;
using System.Collections.Generic;
using System.Linq;

using Azuxiren.MG.Components;
using Azuxiren.MG.Drawing;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;

using static GTT.MovableObject;

namespace GTT
{
	public class WelcomeScreen : IGameStage<CommonDataStruct>
	{
		private enum State : byte { StartScreen, SelectGraphProperties, ArrangeGraph, SelectEdges, SelectAlgorithm, NormalShowcase, FloydShowcase, }
		private State _state;
		private readonly Button _exitButton, _demoButton, _submitNodes, _backButton, _add_edge, _cancel_edge, _next_update, _reset_update;
		private readonly TextBox _welcomeText, _selectNodes, _logText, _actionlogText, _selectAlgo, _select_source, _select_dest;
		private readonly Checkbox _directedbox, _weightedbox;
		private Rectangle _graphDragRegion;
		private readonly Picker _algoPicker, _sourcePicker, _destPicker, _nodeCountPicker;
		private readonly MovableObjectManager _manager;
		private MovableObject[] _movableNodes;
		private readonly Color[] _gdColors;
		private int _nodeCount;
		private byte _nodeSel1 = 255, _nodeSel2 = 255;
		private readonly PlugKeyboard _keyboard;
		private Button[] _nodeButtons;
		private readonly Dictionary<(byte From, byte To), LineObject> _edgelines;
		private byte[,] _edgeMap;
		private GraphDrawing _commonGraphShowcase;
		private GraphDrawingUpdate[] _commonGraphUpdates;
		private FloydWarshallInfo _floydWarshallInfo;
		private FloydWarshallInfoUpdate[] _floydUpdates;
		private int _currentUpdateIndex, _lastUpdateIndex;
		public WelcomeScreen(CommonDataStruct settings)
		{
			_movableNodes = null!;
			_edgeMap = null!;
			_commonGraphUpdates = null!;
			_floydUpdates = null!;
			_floydWarshallInfo = default;
			_commonGraphShowcase = default;
			_currentUpdateIndex = _lastUpdateIndex = 0;
			_nodeButtons = [];
			_edgelines = [];
			_state = State.StartScreen;
			var bound = new Rectangle(0, 0, settings.GameWidth, settings.GameHeight);
			_welcomeText = new(
				new Rectangle(0, 0, bound.Width, bound.Height / 4),
				"  Welcome to Graph\n   Algorithms Demo  ",
				settings.FormalFont,
				settings.ComponentTextColor,
				TextBox.TextAlignment.Centered
			);
			bound = new Rectangle(0, _welcomeText.Bounds.Bottom, bound.Width, bound.Height - _welcomeText.Bounds.Bottom);
			Rectangle menu_region_x = DrawingExtensions.SetCenter(
				bound.Center,
				new(140, 60)
			);
			_exitButton = new Button(
				settings,
				"Exit",
				new Rectangle(
					menu_region_x.X,
					menu_region_x.Bottom + 20,
					menu_region_x.Width,
					menu_region_x.Height
				)
			);
			_demoButton = new(settings, "Begin", menu_region_x);
			Rectangle menu_region_y = menu_region_x;
			menu_region_y.Width += 50;
			menu_region_y.Y -= 2 * menu_region_x.Height + 20;
			_directedbox = new(settings, "Directed", menu_region_y);
			menu_region_y.Y += menu_region_x.Height + 10;
			_weightedbox = new(settings, "Weighted", menu_region_y);
			menu_region_y.Y += menu_region_x.Height + 10;
			menu_region_y.X -= menu_region_x.Width + 10;
			menu_region_y.Width -= 50;
			_selectNodes = new(
				new(
					menu_region_y.X - 150,
					menu_region_y.Y - 150,
					menu_region_y.Width + 150,
					menu_region_y.Height + 300
				),
				"Number of nodes",
				settings.Font,
				settings.ComponentTextColor
			);
			_gdColors = settings.GdColors;
			menu_region_y.X += menu_region_x.Width + 20;
			_nodeCountPicker = new Picker([.. Enumerable.Range(2, 25).Select(x => x.ToString())], menu_region_y, settings);
			menu_region_y.X -= 10;
			menu_region_y.Y += 2 * menu_region_x.Height + 40;
			_submitNodes = new(settings, "Submit", menu_region_y);
			menu_region_y.Y += menu_region_x.Height + 20;
			_backButton = new Button(settings, "Back", menu_region_y);
			_graphDragRegion = new(0, 0, settings.GameWidth * 8 / 10, _submitNodes.Bounds.Top);
			_manager = new() { Region = _graphDragRegion };
			menu_region_y.X = _graphDragRegion.Right;
			menu_region_y.Width = settings.GameWidth - _graphDragRegion.Right;
			menu_region_y.Y = settings.GameHeight - (_graphDragRegion.Height / 2);
			menu_region_y.Height = _graphDragRegion.Height / 2;
			_keyboard = new PlugKeyboard(settings, menu_region_y);
			var subbounds = _submitNodes.Bounds;
			subbounds.X = menu_region_y.X - _submitNodes.Bounds.Width;
			_submitNodes.Set(subbounds);
			subbounds = _backButton.Bounds;
			subbounds.X = _submitNodes.Bounds.X;
			_backButton.Set(subbounds);
			menu_region_y.Y /= 2;
			menu_region_y.Height = menu_region_y.Y / 4;
			_add_edge = new(settings, "OK", menu_region_y);
			Rectangle act_log_dest = new(
				menu_region_y.X,
				0,
				settings.GameWidth - menu_region_y.X,
				menu_region_y.Y
			);
			_next_update = new(settings, "Next", menu_region_y);
			menu_region_y.Y += 2 * menu_region_y.Height;
			_cancel_edge = new(settings, "Cancel", menu_region_y);
			_reset_update = new(settings, "Reset", menu_region_y);
			menu_region_y = new Rectangle(
				0,
				_submitNodes.Bounds.Y,
				_submitNodes.Bounds.X,
				settings.GameHeight - _graphDragRegion.Height
			);
			_logText = new TextBox(
				menu_region_y,
				"",
				settings.Font,
				settings.ComponentTextColor
			);
			_actionlogText = new TextBox(
				act_log_dest,
				"",
				settings.Font,
				settings.ComponentTextColor
			);
			Rectangle algobd = DrawingExtensions.SetCenter(
				new(settings.GameWidth / 2, settings.GameHeight / 5),
				new(_graphDragRegion.Width / 2, 60)
			);

			_selectAlgo = new TextBox(
				algobd,
				"Select the algorithm",
				settings.Font,
				settings.ComponentTextColor
			);
			algobd.Y += 80;
			_algoPicker = new(
				[
					"Depth First Search",
					"Breadth First Search",
					"Dijkstra Algorithm",
					"Floyd-Warshall Algorithm",
					"Kruskal's Algorithm"
				],
				algobd,
				settings
			);
			_nodeCount = 26;
			string[] source_options = new string[_nodeCount], dest_options = new string[_nodeCount + 1];
			for (byte j = 0; j < _nodeCount; j++)
			{
				source_options[j] = dest_options[j + 1] = Algorithms.GetLabel(j);
			}
			dest_options[0] = "None";
			algobd.Y += 80;
			_select_source = new TextBox(
				algobd,
				"Source: ",
				settings.Font,
				settings.ComponentTextColor
			);
			algobd.Y += 80;
			_sourcePicker = new(source_options, algobd, settings);
			algobd.Y += 80;
			_select_dest = new TextBox(
				algobd,
				"Destination: ",
				settings.Font,
				settings.ComponentTextColor
			);
			algobd.Y += 80;
			_destPicker = new(dest_options, algobd, settings);
		}
		public GameUpdateResult Update(GameTime gt, ref CommonDataStruct settings)
		{
			settings.Input.Update();
			int i;
			switch (_state)
			{
				#region 1-Update
				case State.StartScreen:
					if (_exitButton.Update(gt) == BaseButton.BaseButtonState.JustReleased)
					{
						return GameUpdateResult.ExitRequest;
					}
					if (_demoButton.Update(gt) == BaseButton.BaseButtonState.JustPressed)
					{
						_state = State.SelectGraphProperties;
						_nodeCountPicker.ResetIndex();
					}
					break;
				#endregion
				#region 2-Update
				case State.SelectGraphProperties:
					_nodeCountPicker.Update(gt);
					_directedbox.Update(gt);
					_weightedbox.Update(gt);
					if (_submitNodes.Update(gt) == BaseButton.BaseButtonState.JustPressed)
					{
						_manager.CurrentlyLocked = 0;
						_manager.TotalElements = 0;
						_nodeCount = _nodeCountPicker.Index + 2;
						_movableNodes = new MovableObject[_nodeCount];
						_nodeButtons = new Button[_nodeCount];
						Rectangle pos = new(10, 10, 40, 40);
						for (byte x = 0; x < _nodeCount; x++)
						{
							string title = Algorithms.GetLabel(x);
							_movableNodes[x] = new(
								_manager,
								title,
								pos,
								settings.Circle,
								settings
							);
							_nodeButtons[x] = new Button(settings, title, pos, circle: true);
							pos.X += 50;
							if (pos.Right >= _graphDragRegion.Width)
							{
								pos.X = 10;
								pos.Y += 50;
							}
						}
						_logText.Text = "Drag and arrange the\n nodes of the graph";
						_state = State.ArrangeGraph;
					}
					break;
				#endregion
				#region 3-Update
				case State.ArrangeGraph:
					for (i = _nodeCount - 1; i >= 0; i--)
					{
						_movableNodes[i].Update();
					}
					if (_submitNodes.Update(gt) == BaseButton.BaseButtonState.JustPressed)
					{
						for (byte x = 0; x < _nodeCount; x++)
						{
							_nodeButtons?[x].Set(_movableNodes[x].Bounds);
						}
						_edgeMap = new byte[_nodeCount, _nodeCount];
						_edgelines.Clear();
						_state = State.SelectEdges;
					}
					break;
				#endregion
				#region 4-Update
				case State.SelectEdges:
					for (i = 0; i < _nodeCount; i++)
					{
						if (_nodeButtons[i].Update(gt) == BaseButton.BaseButtonState.JustPressed)
						{
							if (_nodeSel1 == 255)
								_nodeSel1 = (byte)i;
							else if (_nodeSel2 == 255)
							{
								_nodeSel2 = (byte)i;
								if (_nodeSel2 == _nodeSel1)
								{
									_nodeSel1 = _nodeSel2 = 255;
								}
								else _keyboard.Value = _weightedbox.IsChecked ? 0 : 1;
							}
							else { _nodeSel2 = (byte)i; }
						}
					}
					if (_nodeSel1 == 255)
						_logText.Text = "Select the source node          ";
					else if (_nodeSel2 == 255)
						_logText.Text = "Select the destination node     ";
					else
					{
						string keytext = $"({Algorithms.GetLabel(_nodeSel1)},{Algorithms.GetLabel(_nodeSel2)})";
						if (_weightedbox.IsChecked)
						{
							_keyboard.Update(gt);
							_logText.Text = $"Select the weight and press OK\nto add the edge {keytext}";
						}
						else
							_logText.Text = $"Select OK to add edge {keytext}      ";
						if (_add_edge.Update(gt) == BaseButton.BaseButtonState.JustPressed)
						{
							var key = (_nodeSel1, _nodeSel2);
							keytext = $"({Algorithms.GetLabel(key._nodeSel1)},{Algorithms.GetLabel(key._nodeSel2)})";
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
									_actionlogText.Text = $"Edge {keytext} \nis removed from\nthe graph";
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
										_edgelines.Add(key, CreateLine(key, settings));
									}
									_actionlogText.Text = $"Edge is updated in the graph.\n{keytext} with weight {_keyboard.Value}";
									_actionlogText.TextColor = Color.DarkGreen;
								}
								else
								{
									_actionlogText.Text = "Invalid weight.\n Weight should be\nbetween 1 and 255\n Enter 0 to\nremove existing\n  edge  ";
									_actionlogText.TextColor = Color.DarkRed;
								}
							}
							else
							{
								if (_keyboard.Value == 0)
								{
									_actionlogText.Text = "Edge does not\n exist, and thus\ncannot be removed!";
									_actionlogText.TextColor = Color.DarkRed;
								}
								else if (_keyboard.Value < 0 || _keyboard.Value > 255)
								{
									_actionlogText.Text = "Invalid weight.\n Weight should be\nbetween 1 and 255\n Enter 0 to\nremove existing\n  edge  ";
									_actionlogText.TextColor = Color.DarkRed;
								}
								else
								{
									_edgeMap[_nodeSel1, _nodeSel2] = (byte)_keyboard.Value;
									if (_directedbox.IsChecked == false)
										_edgeMap[_nodeSel2, _nodeSel1] = _edgeMap[_nodeSel1, _nodeSel2];
									_edgelines.Add(key, CreateLine(key, settings));
									_actionlogText.Text = $"Edge \n({Algorithms.GetLabel(key._nodeSel1)}, {Algorithms.GetLabel(key._nodeSel2)})\n with weight {_keyboard.Value}\n is added";
									_actionlogText.TextColor = Color.DarkGreen;
								}
							}
							_nodeSel1 = _nodeSel2 = 255;
						}
						if (_cancel_edge.Update(gt) == BaseButton.BaseButtonState.JustPressed)
							_nodeSel1 = _nodeSel2 = 255;
					}
					if (_submitNodes.Update(gt) == BaseButton.BaseButtonState.JustPressed)
					{
						_algoPicker.SetLimit((byte)(_directedbox.IsChecked ? 3 : 4));
						_sourcePicker.SetLimit((byte)(_nodeCount - 1));
						_destPicker.SetLimit((byte)_nodeCount);
						_commonGraphShowcase = new([.. _movableNodes.Select(x => x.Bounds)], _edgelines, settings);
						_state = State.SelectAlgorithm;
					}
					break;
				#endregion
				#region 5-Update
				case State.SelectAlgorithm:
					_algoPicker.Update(gt);
					_sourcePicker.Update(gt);
					_destPicker.Update(gt);
					if (_submitNodes.Update(gt) == BaseButton.BaseButtonState.JustPressed)
					{
						switch (_algoPicker.Index)
						{
							case 0:
								_commonGraphUpdates = [
									.. Algorithms.DepthFirstSearch(
										(byte)_nodeCount,
										_edgeMap,
										_sourcePicker.Index,
										(byte)(_destPicker.Index - 1),
										_gdColors
									)
								];
								goto default;
							case 1:
								_commonGraphUpdates = [
									.. Algorithms.BreadthFirstSearch(
										(byte)_nodeCount,
										_edgeMap,
										_sourcePicker.Index,
										(byte)(_destPicker.Index - 1),
										_gdColors
									)
								];
								goto default;
							case 2:
								_commonGraphUpdates = [
									.. Algorithms.Dijkstra(
										(byte)_nodeCount,
										_edgeMap,
										_sourcePicker.Index,
										(byte)(_destPicker.Index - 1),
										_gdColors
									)
								];
								goto default;
							case 4:
								_commonGraphUpdates = [
									.. Algorithms.Kruskal(
										(byte)_nodeCount,
										_edgeMap,
										_gdColors
									)
								];
								goto default;
							case 3:
								_floydUpdates = [.. Algorithms.FloydWarshall((byte)_nodeCount, _edgeMap)];
								ResetFloyd(settings);
								_state = State.FloydShowcase;
								break;
							default:
								ResetAlgo();
								_state = State.NormalShowcase;
								break;
						}
					}
					break;
				#endregion
				#region 6-Update
				case State.NormalShowcase:
					if (_next_update.Update(gt) == BaseButton.BaseButtonState.JustPressed)
					{
						_logText.Text = _commonGraphUpdates[_currentUpdateIndex].UpdateLog;
						_commonGraphShowcase.Update(_commonGraphUpdates[_currentUpdateIndex++]);
						if (_currentUpdateIndex == _lastUpdateIndex) _next_update.EnabledProperty = false;
					}
					if (_reset_update.Update(gt) == BaseButton.BaseButtonState.JustPressed)
						ResetAlgo();
					break;
				#endregion
				#region 7-Update
				case State.FloydShowcase:
					if (_next_update.Update(gt) == BaseButton.BaseButtonState.JustPressed)
					{
						_logText.Text = _floydUpdates[_currentUpdateIndex].Log;
						_floydWarshallInfo.Update(_floydUpdates[_currentUpdateIndex++]);
						if (_currentUpdateIndex == _lastUpdateIndex) _next_update.EnabledProperty = false;
					}
					if (_reset_update.Update(gt) == BaseButton.BaseButtonState.JustPressed)
						ResetFloyd(settings);
					break;
					#endregion
			}
			if (_state != State.StartScreen && _backButton.Update(gt) == BaseButton.BaseButtonState.JustPressed)
			{
				if (_state == State.FloydShowcase) _state--;
				_state--;
				_logText.Text = "";
				_actionlogText.Text = "";
			}
			return GameUpdateResult.NoAction;
		}
		public void Draw(GameTime gt, in RenderTargetDrawer drawer_tgt, in CommonDataStruct settings)
		{
			byte i;
			var patch = settings.Patch;
			var color = settings.ClearColor;
			drawer_tgt.DrawToTarget(
				(drawer) =>
				{

					switch (_state)
					{
						case State.StartScreen:
							_welcomeText.Draw(drawer);
							_demoButton.Draw(drawer);
							_exitButton.Draw(drawer);
							break;
						case State.SelectGraphProperties:
							_selectNodes.Draw(drawer);
							_directedbox.Draw(drawer);
							_weightedbox.Draw(drawer);
							_nodeCountPicker.Draw(drawer);
							break;
						case State.ArrangeGraph:
							drawer.Draw(
								patch,
								_graphDragRegion,
								color: color
							);
							for (i = 0; i < _nodeCount; i++)
							{
								_movableNodes[i].Draw(drawer);
							}
							_logText.Draw(drawer);
							break;
						case State.SelectEdges:
							drawer.Draw(
								patch,
								_graphDragRegion,
								color: color
							);
							foreach (var edge in _edgelines)
							{
								edge.Value.Draw(drawer);
							}
							for (i = 0; i < _nodeCount; i++)
							{
								_nodeButtons[i].Draw(drawer);
							}
							if (_nodeSel1 != 255 && _nodeSel2 != 255)
							{
								if (_weightedbox.IsChecked) _keyboard.Draw(drawer);
								_add_edge.Draw(drawer);
								_cancel_edge.Draw(drawer);
							}
							_logText.Draw(drawer);
							_actionlogText.Draw(drawer);
							break;
						case State.SelectAlgorithm:
							_selectAlgo.Draw(drawer);
							_algoPicker.Draw(drawer);
							if (_algoPicker.Index < 3)
							{
								_sourcePicker.Draw(drawer);
								_destPicker.Draw(drawer);
								_select_source.Draw(drawer);
								_select_dest.Draw(drawer);
							}
							break;
						case State.NormalShowcase:
							drawer.Draw(
								patch,
								_graphDragRegion,
								color: color
							);
							_commonGraphShowcase.Draw(drawer);
							goto default;
						case State.FloydShowcase:
							drawer.Draw(
								patch,
								_graphDragRegion,
								color: color
							);
							_floydWarshallInfo.Draw(drawer);
							goto default;
						default:
							_next_update.Draw(drawer);
							_reset_update.Draw(drawer);
							_logText.Draw(drawer);
							break;
					}
					if (_state != State.StartScreen)
					{
						_backButton.Draw(drawer);
						if (_state < State.NormalShowcase)
							_submitNodes.Draw(drawer);
					}
				}
			);
		}
		private void ResetAlgo()
		{
			_commonGraphShowcase.Reset();
			_lastUpdateIndex = _commonGraphUpdates.Length;
			_currentUpdateIndex = 1;
			_logText.Text = _commonGraphUpdates[0].UpdateLog;
			_commonGraphShowcase.Update(_commonGraphUpdates[0]);
			_next_update.EnabledProperty = true;
		}
		private LineObject CreateLine(
			(byte from, byte to) key,
			CommonDataStruct settings
		) => new(
				settings,
				_movableNodes[key.from].Bounds.Center,
				_movableNodes[key.to].Bounds.Center,
				_directedbox.IsChecked,
				_keyboard.Value.ToString(),
				5
			);
		private void ResetFloyd(CommonDataStruct settings)
		{
			_lastUpdateIndex = _floydUpdates.Length;
			_floydWarshallInfo = new FloydWarshallInfo(_edgeMap, _graphDragRegion, settings);
			_currentUpdateIndex = 1;
			_logText.Text = _floydUpdates[0].Log;
			_next_update.EnabledProperty = true;
		}
	}
}