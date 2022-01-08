using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
namespace GTT
{
	public class LineObject
	{
		protected readonly static Vector2 Origin = new(0f, 0.5f);
		protected readonly static Texture2D tex = GameApp.CurrentGame.patch;
		protected readonly static Texture2D head = GameApp.CurrentGame.triangle;
		protected readonly static Vector2 arrowcenter = new(head.Width / 2, head.Height - 1);
		public Vector2 point1, Scale;
		public Rectangle arrow;
		public float Angle, arrowAngle;
		public LineObject(Point start, Point end, float thickness)
		{
			point1 = Scale = Vector2.Zero;
			Angle = 0;
			arrow = Rectangle.Empty;
			Compute(start, end, thickness);
		}
		public void Compute(Point start, Point end, float thickness = 1f)
		{
			point1 = start.ToVector2();
			var point2 = end.ToVector2();
			byte len = 5;
			arrow.Width = arrow.Height = 2 * len;
			arrow.X = end.X;
			arrow.Y = end.Y;
			Scale = new Vector2(Vector2.Distance(point1, point2), thickness);
			Angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float radian90 = (float)(Math.PI / 2);
			arrowAngle = Angle + radian90;
		}
		public void Draw(SpriteBatch batch, Color color)
		{
			batch.Draw(tex, point1, null, color, Angle, Origin, Scale, SpriteEffects.None, 0);
			batch.Draw(head, arrow, null, color, arrowAngle, arrowcenter, SpriteEffects.None, 0);
		}
	}

}