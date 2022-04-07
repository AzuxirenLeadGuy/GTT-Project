using Azuxiren.MG;
using Azuxiren.MG.Menu;

using Microsoft.Xna.Framework;
namespace GTT
{
	public class PlugKeyboard : IMenuItem
	{
		private TextBox _box;
		private readonly Button[] _numericPads;
		private int _value;
		public int Value { get => _value; set => _box.Text = (_value = value).ToString(); }
		public PlugKeyboard(Rectangle bds, byte defaultValue = 0)
		{
			_numericPads = new Button[11];
			for (int i = 0; i < 10; i++)
			{
				_numericPads[i] = new Button(Rectangle.Empty, i.ToString());
			}
			_numericPads[10] = new Button(Rectangle.Empty, "Clear");
			_value = defaultValue;
			_box = new TextBox(Rectangle.Empty, _value.ToString(), GameApp.CommonData.Font);
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
				if (_numericPads[i].ClickedOnUpdate(gt))
				{
					_value = (_value * 10) + i;
					change = true;
				}
			}
			if (_numericPads[10].ClickedOnUpdate(gt))
			{
				_value = 0;
				change = true;
			}
			if (change)
			{
				_box.Text = _value.ToString();
			}
		}
	}
}