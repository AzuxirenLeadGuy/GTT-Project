using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GTT
{
	public class LineObject
	{
		protected readonly static Vector2 Origin = new(0f, 0.5f);
		protected readonly static Texture2D Tex = GameApp.CommonData.Patch;
		protected readonly static Texture2D Head = GameApp.CommonData.Triangle;
		protected readonly static Vector2 Arrowcenter = new(Head.Width / 2, Head.Height - 1);
		private Vector2 _point1, _scale;
		private Rectangle _arrow;
		private float _angle, _arrowAngle;
		public LineObject(Point start, Point end, float thickness = 1f)
		{
			_point1 = _scale = Vector2.Zero;
			_angle = 0;
			_arrow = Rectangle.Empty;
			Compute(start, end, thickness);
		}
		public void Compute(Point start, Point end, float thickness = 1f)
		{
			_point1 = start.ToVector2();
			var point2 = end.ToVector2();
			byte len = 5;
			_arrow.Width = _arrow.Height = 2 * len;
			_arrow.X = end.X;
			_arrow.Y = end.Y;
			_scale = new Vector2(Vector2.Distance(_point1, point2), thickness);
			_angle = (float)Math.Atan2(point2.Y - _point1.Y, point2.X - _point1.X);
			float radian90 = (float)(Math.PI / 2);
			_arrowAngle = _angle + radian90;
		}
		public void Draw(SpriteBatch batch, Color color)
		{
			batch.Draw(Tex, _point1, null, color, _angle, Origin, _scale, SpriteEffects.None, 0);
			batch.Draw(Head, _arrow, null, color, _arrowAngle, Arrowcenter, SpriteEffects.None, 0);
		}
	}

}