using Laba1.Controller;
using Laba1.Model;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Point = Laba1.Model.Point;

namespace Laba1
{
	public partial class MainPage : ContentPage
	{
		private SKColor _backgroundColor = Color.Black.ToSKColor();

		private List<Figure> _figureList;
		public List<Figure> FigureList
		{
			get { return _figureList; }
			set
			{
				_figureList = value;
				canvasView.InvalidateSurface();
			}
		}

		private List<SKPath> _prevFigurePath = new List<SKPath>();
		private List<Figure> _prevFigure = new List<Figure>();

		public MainPage()
		{
			InitializeComponent();
			_figureList = new List<Figure>();
			BindingContext = new FigureController(this, 60);
		}

		public void UpdateCanvas()
		{
			canvasView.InvalidateSurface();
		}

		private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
		{
			SKImageInfo info = args.Info;
			SKSurface surface = args.Surface;
			SKCanvas canvas = surface.Canvas;



			if (_prevFigurePath == null || _prevFigurePath.Count == 0)
			{
				canvas.Clear(_backgroundColor);
			}
			else
			{
				ClearCanvas(canvas);
			}

			_prevFigurePath.Clear();
			_prevFigure.Clear();
			foreach (Figure figure in _figureList)
			{
				SKPaint borderPaint = new SKPaint
				{
					Style = SKPaintStyle.Stroke,
					Color = figure.BorderColor.ToSKColor(),
					StrokeWidth = figure.BorderWidth,
					IsAntialias = false,
					StrokeCap = SKStrokeCap.Round
				};

				SKPaint fillPaint = new SKPaint
				{
					Style = SKPaintStyle.Fill,
					Color = figure.FillColor.ToSKColor(),
					IsAntialias = false,
				};

				switch (figure.FigureType)
				{
					case FigureType.Angular:
						DrawAngular(figure, canvas, borderPaint, fillPaint);
						break;
					case FigureType.Smooth:
						DrawSmooth(figure as Oval, canvas, borderPaint, fillPaint);
						break;
				}

			}
		}

		private void ClearCanvas(SKCanvas canvas)
		{
			for (int i = 0; i < _prevFigurePath.Count; i++)
			{
				SKPaint paint = new SKPaint
				{
					Style = SKPaintStyle.StrokeAndFill,
					Color = _backgroundColor,
					StrokeWidth = _prevFigure[i].BorderWidth,
					IsAntialias = false,
					StrokeCap = SKStrokeCap.Round
				};

				SKPath figurePath = _prevFigurePath[i];
				canvas.DrawPath(figurePath, paint);
			}
		}

		private void DrawAngular(Figure figure, SKCanvas canvas, SKPaint borderPaint, SKPaint fillPaint)
		{
			SKPath path = new SKPath();
			Point pos = figure.Position;
			SKPoint[] skPointsList = new SKPoint[figure.Points.Length + 1];
			for (int i = 0; i < figure.Points.Length; i++)
			{
				Point p = figure.Points[i];
				SKPoint skP = new SKPoint(p.X + pos.X, p.Y + pos.Y);
				skPointsList[i] = skP;
				if (i == 0)
				{
					path.MoveTo(skP);
				}
				else
				{
					path.LineTo(new SKPoint(p.X + pos.X, p.Y + pos.Y));
				}
			}
			skPointsList[skPointsList.Length - 1] = skPointsList[0];
			path.LineTo(skPointsList[0]);
			_prevFigurePath.Add(path);
			_prevFigure.Add(figure);
			canvas.DrawPath(path, fillPaint);
			canvas.DrawPath(path, borderPaint);
		}

		private void DrawSmooth(Oval figure, SKCanvas canvas, SKPaint borderPaint, SKPaint fillPaint)
		{
			Point pos = figure.Position;
			SKRect sKRect = new SKRect(pos.X, pos.Y, pos.X + figure.Width, pos.Y + figure.Height);
			SKPath path = new SKPath();
			path.AddOval(sKRect);
			_prevFigurePath.Add(path);
			_prevFigure.Add(figure);
			canvas.DrawPath(path, fillPaint);
			canvas.DrawPath(path, borderPaint);
		}
	}
}
