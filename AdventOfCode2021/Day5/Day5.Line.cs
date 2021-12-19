using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode2021.Day5
{
    public partial class Day5
    {
        private struct Line : IEnumerable<Point>
        {
            public Line(Point Start, Point End)
            {
                this.Start = Start;
                this.End = End;
            }

            public Point Start { get; }
            public Point End { get; }

            public bool Horizontal => Start.Y == End.Y;
            public bool Vertical => Start.X == End.X;
            public bool Diagonal => Start.X != End.X && Start.Y != End.Y;

            public IEnumerator<Point> GetEnumerator()
            {
                if (Diagonal == false)
                {
                    int minX = Math.Min(Start.X, End.X);
                    int minY = Math.Min(Start.Y, End.Y);

                    int maxX = Math.Max(Start.X, End.X);
                    int maxY = Math.Max(Start.Y, End.Y);

                    int start = Horizontal ? minX : minY;
                    int end = Horizontal ? maxX : maxY;

                    for (int i = start; i <= end; i++)
                        yield return new Point(Horizontal ? i : minX, Horizontal ? minY : i);

                    yield break;
                }

                // Flip start and end points if required so we always iterate from top to bottom (Y-axis)
                Point startPoint = Start.Y > End.Y ? End : Start;
                Point endPoint = Start.Y > End.Y ? Start : End;

                int x = startPoint.X;

                // Iterate from top to bottom (Y-axis)
                for (int y = startPoint.Y; y <= endPoint.Y; y++)
                {
                    yield return new Point(x, y);

                    // Increment or decrement X depending on direciton.
                    x = startPoint.X > endPoint.X ? x - 1 : x + 1;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}