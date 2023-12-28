// See https://aka.ms/new-console-template for more information
using CSharpPlayground.Enum;
using System.Runtime.CompilerServices;

string[] line = Console.ReadLine().Split();
var n = int.Parse(line[0]);
var l = int.Parse(line[1]);

var fences = new List<(Point, Point)>();
var route = new List<Point>()
{
    new Point(0, 0),
    new Point(l, l)
};

for (var i = 0; i < n; i++)
{
    line = Console.ReadLine().Split();
    var a = double.Parse(line[0]);
    var b = double.Parse(line[1]);
    var c = double.Parse(line[2]);
    var d = double.Parse(line[3]);

    var p1 = new Point(a, b);
    var p2 = new Point(c, d);
    fences.Add((p1, p2));

    for (int j = 0; j < n; j++)
    {
        if (Intersect(new Line(route[j], route[j + 1]), new Line(p1, p2)))
        {
            Console.WriteLine("NO");
            Console.WriteLine(i);
        }
    }
}

Console.WriteLine("YES");


bool Intersect(Line line1, Line line2)
{
    var s = (line1.Start.X - line1.End.X) * (line2.Start.Y - line1.Start.Y) - (line1.Start.Y - line1.End.Y) * (line2.Start.X - line1.Start.X);
    var t = (line1.Start.X - line1.End.X) * (line2.End.Y - line1.Start.Y) - (line1.Start.Y - line1.End.Y) * (line2.End.X - line1.Start.X);
    return s * t < 0;
}

class Point
{
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; set; }
    public double Y { get; set; }
}

class Line
{
    public Line(Point start, Point end)
    {
        Start = start;
        End = end;
    }

    public Point Start { get; set; }
    public Point End { get; set; }
}