using Azuxiren.MG;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
namespace GTT
{
	public class PlugKeyboard : IMenuItem
	{
		private TextBox _box;
		private readonly Button[] _numericPads;
		internal int Value;
		public PlugKeyboard(Rectangle bds, byte defaultValue = 0)
		{
			_numericPads = new Button[11];
			for (int i = 0; i < 10; i++)
			{
				_numericPads[i] = new Button(Rectangle.Empty, i.ToString());
			}
			_numericPads[10] = new Button(Rectangle.Empty, "Clear");
			Value = defaultValue;
			_box = new TextBox(Rectangle.Empty, Value.ToString(), GameApp.CommonData.Font);
			Set(bds);
		}
		public void Set(Rectangle bds)
		{
			int uw = bds.Width / 3, uh = bds.Height / 5;
			int i, j, x, y, w;
			for (i = 0, y = bds.Y + uh, w = 1; i < 3; i++, y += uh)
			{
				for (j = 0, x = bds.X; j < 3; j++, w++, x += uw)
				{
					_numericPads[w].Set(new Rectangle(x, y, uw, uh));
				}
			}
			x = bds.X;
			y = bds.Y + (4 * uh);
			_numericPads[0].Set(new Rectangle(x, y, uw, uh));
			_numericPads[10].Set(new Rectangle(x + uw, y, 2 * uw, uh));
			_box.Bounds = new Rectangle(bds.X, bds.Y, bds.Width, uh);
		}
		public void Draw(GameTime gt)
		{
			for (int i = 0; i < 11; i++) _numericPads[i].Draw(gt);
			_box.Draw(GameApp.CommonData.Batch);
		}
		public void Update(GameTime gt)
		{
			bool change = false;
			for (int i = 0; i < 10; i++)
			{
				_numericPads[i].Update(gt);
				if (_numericPads[i].State == ComponentState.Release)
				{
					Value = (Value * 10) + i;
					change = true;
				}
			}
			_numericPads[10].Update(gt);
			if (_numericPads[10].State == ComponentState.Release)
			{
				Value = 0;
				change = true;
			}
			if (change)
			{
				_box.Text = Value.ToString();
			}
		}
	}
}