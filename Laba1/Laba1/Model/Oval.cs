using System;
using System.Collections.Generic;
using System.Text;

namespace Laba1.Model
{
	public class Oval : Figure
	{
		public float Height = 50;
		public float Width = 50;

		public Oval(Point position) : base(null, position)
		{
			FigureType = FigureType.Smooth;
		}

		public override ScreenSide SomeSide(Point newPosition, int maxX, int maxY)
		{
			if (newPosition.X + Width >= maxX) return ScreenSide.Right;
			if (newPosition.X <= 0) return ScreenSide.Left;
			if (newPosition.Y + Height >= maxY) return ScreenSide.Top;
			if (newPosition.Y <= 0) return ScreenSide.Bottom;
			return ScreenSide.None;
		}
	}
}
