using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Laba1.Model
{
	public enum ScreenSide
	{
		Left,
		Right,
		Top,
		Bottom,
		None
	}

	public enum FigureType
	{
		Angular,
		Smooth
	}

	public struct Point
	{
		public float X;
		public float Y;

		public Point(float x, float y)
		{
			X = x;
			Y = y;
		}
		public static Point operator *(Point point, float multi)
		{
			return new Point { X = point.X * multi, Y = point.Y * multi };
		}

		public static Point operator +(Point point1, Point point2)
		{
			return new Point { X = point1.X + point2.X, Y = point1.Y + point2.Y };
		}
	}


	public class Figure
	{
		public FigureType FigureType = FigureType.Angular;
		public float BorderWidth = 1;
		public Point[] Points;
		public Point Position;
		public Color BorderColor = Color.Red;
		public Color FillColor = Color.Green;

		public Figure(Point[] points, Point position)
		{
			Points = points;
			Position = position;
		}

		public virtual ScreenSide SomeSide(Point newPosition, int maxX, int maxY)
		{
			foreach (var point in Points)
			{
				Point newPoint = point + newPosition;
				if (newPoint.X >= maxX) return ScreenSide.Right;
				if (newPoint.X <= 0) return ScreenSide.Left;
				if (newPoint.Y >= maxY) return ScreenSide.Top;
				if (newPoint.Y <= 0) return ScreenSide.Bottom;
			}
			return ScreenSide.None;
		}
	}
}
