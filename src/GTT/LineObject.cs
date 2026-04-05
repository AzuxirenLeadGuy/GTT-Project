using System;

using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GTT
{
	public class LineObject
	{
		protected readonly static Vector2 Origin = new(0f, 0.5f);
		protected readonly Texture2D _tex;
		protected readonly Texture2D _head;
		protected readonly SpriteFont _font;
		private Vector2 _point1, _scale;
		private Rectangle _arrow;
		private TextBox? _label;
		private float _angle, _arrowAngle;
		public bool ShowArrow;
		private bool _showLabel;
		public Color ArrowColor;
		public LineObject(CommonDataStruct settings, Point start, Point end, bool keepArrow = true, string? label = null, float thickness = 1f)
		{
			_tex = settings.Patch;
			_head = settings.Triangle;
			_font = settings.Font;
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
			if (label != null)
			{
				var labeldest = _arrow;
				labeldest.Y += _arrow.Height;
				_label = new TextBox(labeldest, label, _font, Color.Black);
			}
		}
		// public string Text { get => (_label?.Text ?? string.Empty); set => _label.Text = value; }
		public void SetLabelColor(Color color)
		{
			if (_label != null)
				_label.TextColor = color;
		}

		public void Draw(IBatchDrawer batch)
		{
			batch.Draw(_tex, _point1, null, ArrowColor, _scale, Origin, _angle);
			if (ShowArrow)
			{
				Vector2 arrowcenter = new(_head.Width / 2, _head.Height - 1);
				batch.Draw(_head, _arrow, null, ArrowColor, arrowcenter, _arrowAngle);
			}
			_label?.Draw(batch);
		}
	}

}