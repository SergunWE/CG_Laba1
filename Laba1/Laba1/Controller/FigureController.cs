using Laba1.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Point = Laba1.Model.Point;

namespace Laba1.Controller
{
	internal class FigureController
	{
		private MainPage _mainPage;
		private int _screenWidth;
		private int _screenHeight;
		private int _frameRate;

		private Thread _moveFigureThread;
		private Thread _colorFigureThread;

		private Random _random;

		private List<Figure> _figures;
		public List<Figure> Figures
		{
			get => _figures;
			set
			{
				_figures = value;
				_mainPage.FigureList = _figures;
			}
		}
		private List<Point> _directionFigures;

		public FigureController(MainPage page, int frameRate = 60)
		{
			_random = new Random((int)DateTime.Now.Ticks);
			_mainPage = page;
			Figures = new List<Figure>() {
				new Figure(new Point[3] {new Point(140, 40), new Point(30, 160), new Point(100, 40)}, new Point(200, 200)){FillColor=Color.LightBlue},
				new Oval(new Point(500, 500)){ Width=90, Height=90, FillColor=Color.LightGoldenrodYellow, BorderWidth=2},
				new Figure(new Point[4] {new Point(140, 40), new Point(30, 160), new Point(100, 40), new Point(250, -40)}, new Point(100, 100)){FillColor=Color.ForestGreen},
				new Oval(new Point(200, 500)){ Width=90, Height=120, FillColor=Color.DeepPink, BorderWidth=8},
			new Figure(new Point[4] {new Point(0, 0), new Point(100, 0), new Point(100, 100), new Point(0, 100)}, new Point(250, 600)){ FillColor=Color.Gray} };
			_directionFigures = new List<Point>(_figures.Count);
			for (int i = 0; i < _figures.Count; i++)
			{
				_directionFigures.Add(new Point(_random.Next(10), _random.Next(10)));
			}

			_screenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
			_screenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;
			_frameRate = frameRate;

			_moveFigureThread = new Thread(ChangeFigurePositionThread);
			_moveFigureThread.Start();

			_colorFigureThread = new Thread(ChangeFigureBorderColorThread);
			_colorFigureThread.Start();
		}

		private void ChangeFigurePositionThread()
		{
			while (true)
			{
				for (int i = 0; i < _figures.Count; i++)
				{
					Figure f = _figures[i];
					ScreenSide screenSide = f.SomeSide(f.Position + _directionFigures[i], _screenWidth, _screenHeight);
					switch (screenSide)
					{
						case ScreenSide.Left:
						case ScreenSide.Right:
							_directionFigures[i] = new Point(_directionFigures[i].X * -1, _directionFigures[i].Y);
							break;
						case ScreenSide.Top:
						case ScreenSide.Bottom:
							_directionFigures[i] = new Point(_directionFigures[i].X, _directionFigures[i].Y * -1);
							break;
						case ScreenSide.None:
							break;
					}
					f.Position += _directionFigures[i] * (60f / _frameRate);
				}
				_mainPage.UpdateCanvas();
				Thread.Sleep(1000 / _frameRate);
			}
		}

		private void ChangeFigureBorderColorThread()
		{
			while (true)
			{
				for (int i = 0; i < _figures.Count; i++)
				{
					_figures[i].BorderColor = Color.FromRgb(_random.Next(256), _random.Next(256), _random.Next(256));
				}
				Thread.Sleep(1000 / _frameRate * 60);
			}
		}
	}
}
