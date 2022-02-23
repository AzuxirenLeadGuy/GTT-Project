using Azuxiren.MG;

using Microsoft.Xna.Framework;

namespace GTT
{
	public class Menu : IMenuItem
	{
		public Menu(Rectangle bds, params IMenuItem[] items)
		{
			_items = items;
			_count = items.Length;
			Set(bds);
		}
		private readonly IMenuItem[] _items;
		private readonly int _count;
		public void Draw(GameTime gt)
		{
			for (int i = 0; i < _count; i++) _items[i].Draw(gt);
		}
		public void Set(Rectangle bds)
		{
			int ih = bds.Height / _count;
			Point cen = new();
			cen.X = (bds.Right + bds.Left) / 2;
			cen.Y = bds.Y + (ih / 2);
			Point dim = new((ih * 5) / 6, (bds.Width * 5) / 6);
			for (int i = 0; i < _count; i++, cen.Y += ih)
			{
				_items[i].Set(Global.SetCenter(cen, dim));
			}
		}
		public void Update(GameTime gt)
		{
			for (int i = 0; i < _count; i++) _items[i].Update(gt);
		}
	}
}