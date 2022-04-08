using System;

using Azuxiren.MG;

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
		private TextBox _label;
		private float _angle, _arrowAngle;
		public bool ShowArrow;
		private bool _showLabel;
		public Color ArrowColor;
		public LineObject(Point start, Point end, bool keepArrow = true, string? label = null, float thickness = 1f)
		{
			_point1 = _scale = Vector2.Zero;
			_angle = 0;
			_arrow = Rectangle.Empty;
			Compute(start, end, keepArrow, label, thickness);
		}
		public void Compute(Point start, Point end, bool keepArrow = true, string? label = null, float thickness = 1f)
		{
			_showLabel = label != null;
			_point1 = start.ToVector2();
			var point2 = end.ToVector2();
			Point mid = new((start.X + end.X) / 2, (start.Y + end.Y) / 2);
			byte len = 15;
			ShowArrow = keepArrow;
			_arrow.Width = _arrow.Height = 2 * len;
			_arrow.X = mid.X;
			_arrow.Y = mid.Y;
			_scale = new Vector2(Vector2.Distance(_point1, point2), thickness);
			_angle = (float)Math.Atan2(point2.Y - _point1.Y, point2.X - _point1.X);
			float radian90 = (float)(Math.PI / 2);
			_arrowAngle = _angle + radian90;
			if (_showLabel)
			{
				var labeldest = _arrow;
				labeldest.Y += _arrow.Height;
				_label = new TextBox(labeldest, label, GameApp.CommonData.Font, Color.Black);
			}
		}
		public string Text { get => _label.Text; set => _label.Text = value; }
		public void SetLabelColor(Color color) => _label.TextColor = color;
		public void Draw(SpriteBatch batch)
		{
			batch.Draw(Tex, _point1, null, ArrowColor, _angle, Origin, _scale, SpriteEffects.None, 0);
			if (ShowArrow)
				batch.Draw(Head, _arrow, null, ArrowColor, _arrowAngle, Arrowcenter, SpriteEffects.None, 0);
			if (_showLabel)
				_label.Draw(batch);
		}
	}

}