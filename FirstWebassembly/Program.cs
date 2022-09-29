using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;



var canvas = new Canvas();
canvas.Redraw();



public partial class Canvas
{
    // 일단 고정
    public const int Width = 1024;
    public const int Height = 768;

    [JSImport("clearCanvas", "main.js")]
    internal static partial void ClearCanvas();

    [JSImport("drawCircle", "main.js")]
    internal static partial void DrawCircle(int x, int y, int radius, string color);

    [JSImport("requestAnimationFrame", "main.js")]
    internal static partial void requestAnimationFrame(
        [JSMarshalAs<JSType.Function>] Action callback);


    class Circle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int XInc { get; set; }
        public int YInc { get; set; }
        public int Radius { get; set; }
        public string Color { get; set; } = "black";
    }

    private Circle[] _circles;

    public Canvas()
    {
        _circles = new Circle[500];
        for (var i = 0; i < _circles.Length; i++)
        {
            _circles[i] = new Circle
            {
                X = Random.Shared.Next(0, Width),
                XInc = Random.Shared.Next() % 2 is 0 ? -1 : 1,
                YInc = Random.Shared.Next() % 2 is 0 ? -1 : 1,
                Y = Random.Shared.Next(0, Height),
                Radius = Random.Shared.Next(10, 20),
                Color = Random.Shared.Next(7) switch
                {
                    0 => "red",
                    1 => "green",
                    2 => "blue",
                    3 => "yellow",
                    4 => "orange",
                    5 => "gray",
                    _ => "purple"
                }
            };
        }
    }

    public void Redraw()
    {
        ClearCanvas();

        for (var i = 0; i < _circles.Length; i++)
        {
            var c = _circles[i];
            DrawCircle(c.X, c.Y, c.Radius, c.Color);

            c.X += c.XInc;
            c.Y += c.YInc;

            if (c.X < 0 || c.X > Width)
                c.XInc *= -1;
            if (c.Y < 0 || c.Y > Height)
                c.YInc *= -1;
        }

        requestAnimationFrame(Redraw);
    }
}
