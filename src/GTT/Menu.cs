using Azuxiren.MG.Drawing;

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
		public void Set(Rectangle bds)
		{
			int ih = bds.Height / _count;
			Point cen = new()
			{
				X = (bds.Right + bds.Left) / 2,
				Y = bds.Y + (ih / 2)
			};
			Point dim = new((ih * 5) / 6, (bds.Width * 5) / 6);
			for (int i = 0; i < _count; i++, cen.Y += ih)
			{
				_items[i].Set(Azuxiren.MG.Drawing.DrawingExtensions.SetCenter(cen, dim));
			}
		}
		public static void Set(Rectangle bds, params IMenuItem[] items)
		{
			var count = items.Length;
			int ih = bds.Height / count;
			Point cen = new()
			{
				X = (bds.Right + bds.Left) / 2,
				Y = bds.Y + (ih / 2)
			};
			Point dim = new((ih * 5) / 6, (bds.Width * 5) / 6);
			for (int i = 0; i < count; i++, cen.Y += ih)
			{
				items[i].Set(Azuxiren.MG.Drawing.DrawingExtensions.SetCenter(cen, dim));
			}
		}
	}
}