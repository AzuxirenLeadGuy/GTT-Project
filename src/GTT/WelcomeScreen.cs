using System.Linq;

using Azuxiren.MG;

using Microsoft.Xna.Framework;

namespace GTT
{
	public class WelcomeScreen : IScreen
	{
		private enum State : byte { StartScreen, SelectGraphProperties, ArrangeGraph, SelectEdges, SelectAlgorithm }
		private State _state;
		private readonly Button _exitButton, _demoButton, _submitNodes, _backButton;
		private readonly TextBox _welcomeText, _selectNodes;
		private readonly Checkbox _directedbox, _weightedbox;
		private Rectangle GraphDragRegion, LogRegion, OKRegion;
		private readonly Picker _nodeCountPicker;
		private readonly MovableObjectManager _manager;
		private MovableObject[] _movableNodes;
		private int NodeCount;
		public WelcomeScreen()
		{
			_state = State.StartScreen;
			var bound = GameApp.CommonData.GameScreen;
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
			_selectNodes = new(y, "Number of nodes", GameApp.CommonData.Font);
			y.X += x.Width + 20;
			_nodeCountPicker = new Picker(Enumerable.Range(1, 19).Select(x => x.ToString()).ToArray(), y);
			y.X -= 10;
			y.Y += x.Height + 20;
			_submitNodes = new(y, "Submit");
			y.Y += x.Height + 20;
			_backButton = new Button(y, "Back");
			_backButton.OnRelease += (o, e) => _state--;
			GraphDragRegion = new(0, 0, GameApp.CommonData.GameScreen.Width * 8 / 10, _backButton.Bounds.Top);
			_manager = new() { Region = GraphDragRegion };
			_submitNodes.OnRelease += (o, e) =>
			{
				_manager.CurrentlyLocked = 0;
				_manager.TotalElements = 0;
				NodeCount = _nodeCountPicker.Index + 1;
				_movableNodes = new MovableObject[NodeCount];
				for (int i = 0; i < NodeCount; i++)
				{
					_movableNodes[i] = new MovableObject(_manager, (i + 1).ToString(), new Rectangle(10, 10, 100, 100), Color.White);
				}
				_state = State.ArrangeGraph;
			};
		}
		public void LoadContent() { }
		public void Update(GameTime gt)
		{
			if (_state != State.StartScreen) _backButton.Update(gt);
			switch (_state)
			{
				case State.StartScreen:
					_exitButton.Update(gt);
					_demoButton.Update(gt);
					break;
				case State.SelectGraphProperties:
					_directedbox.Update(gt);
					_weightedbox.Update(gt);
					_submitNodes.Update(gt);
					_nodeCountPicker.Update(gt);
					break;
				case State.ArrangeGraph:
					for (int i = NodeCount - 1; i >= 0; i--)
					{
						_movableNodes[i].Update();
					}
					break;
			}
		}
		public void Draw(GameTime gt)
		{
			switch (_state)
			{
				case State.StartScreen:
					_welcomeText.Draw(GameApp.CommonData.Batch);
					_demoButton.Draw(gt);
					_exitButton.Draw(gt);
					break;
				case State.SelectGraphProperties:
					_directedbox.Draw(gt);
					_weightedbox.Draw(gt);
					_selectNodes.Draw(GameApp.CommonData.Batch);
					_submitNodes.Draw(gt);
					_backButton.Draw(gt);
					_nodeCountPicker.Draw(gt);
					break;
				case State.ArrangeGraph:
					GameApp.CommonData.Batch.Draw(GameApp.CommonData.Patch, GraphDragRegion, Color.Yellow);
					for (int i = 0; i < NodeCount; i++)
					{
						_movableNodes[i].Draw();
					}
					break;
			}
			if (_state != State.StartScreen) _backButton.Draw(gt);
		}
	}
}