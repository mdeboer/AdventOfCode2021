using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day5
{
    public partial class Day5 : IPuzzle
    {
        public async Task<(IEnumerable<string>, TimeSpan)> SolveAsync(CancellationToken cancellationToken = default)
        {
            // All lines from input.
            (List<Line> lines, Point bottomRight) = await ReadInput(
                Path.Combine(AppContext.BaseDirectory, "Day5", "input.txt"),
                cancellationToken);

            Stopwatch sw = Stopwatch.StartNew();

            string[] results =
            {
                Part1(lines, bottomRight, cancellationToken),
                Part2(lines, bottomRight, cancellationToken)
            };

            sw.Stop();

            return (results, sw.Elapsed);
        }

        private string Part1(List<Line> lines, Point bottomRight, CancellationToken cancellationToken)
        {
            int[,] matrix = new int[bottomRight.Y + 1, bottomRight.X + 1];

            // Draw lines in the matrix Neo.
            foreach (Line line in lines)
            {
                if (line.Diagonal)
                    continue;

                foreach (Point point in line)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    matrix[point.Y, point.X]++;
                }
            }

            // Get the number of points where at least two lines overlap.
            int dangerousOverlap = matrix.Cast<int>().Count(i => i >= 2);

            return dangerousOverlap.ToString();
        }

        private string Part2(List<Line> lines, Point bottomRight, CancellationToken cancellationToken)
        {
            int[,] matrix = new int[bottomRight.Y + 1, bottomRight.X + 1];

            // Draw lines in the matrix Neo.
            foreach (Line line in lines)
            foreach (Point point in line)
            {
                cancellationToken.ThrowIfCancellationRequested();

                matrix[point.Y, point.X]++;
            }

            // Get the number of points where at least two lines overlap.
            int dangerousOverlap = matrix.Cast<int>().Count(i => i >= 2);

            return dangerousOverlap.ToString();
        }

        private async Task<(List<Line>, Point)> ReadInput(string file, CancellationToken cancellationToken = default)
        {
            List<Line> lines = new();

            // Bottom right coordinates (0,0 -> x,x)
            int maxX = 0;
            int maxY = 0;

            await using (FileStream fs = File.OpenRead(file))
            {
                using (StreamReader sr = new(fs, Encoding.ASCII))
                {
                    Regex rowRegex = new(@"^([0-9]+)\,([0-9]+) \-\> ([0-9]+)\,([0-9]+)", RegexOptions.Compiled);

                    while (sr.EndOfStream == false && cancellationToken.IsCancellationRequested == false)
                    {
                        string? row = await sr.ReadLineAsync();

                        if (string.IsNullOrWhiteSpace(row))
                            continue;

                        Match rowMatch = rowRegex.Match(row);

                        maxX = Math.Max(maxX,
                            Math.Max(int.Parse(rowMatch.Groups[1].Value), int.Parse(rowMatch.Groups[3].Value)));

                        maxY = Math.Max(maxY,
                            Math.Max(int.Parse(rowMatch.Groups[2].Value), int.Parse(rowMatch.Groups[4].Value)));

                        lines.Add(new Line(
                                new Point(
                                    int.Parse(rowMatch.Groups[1].Value),
                                    int.Parse(rowMatch.Groups[2].Value)
                                ),
                                new Point(
                                    int.Parse(rowMatch.Groups[3].Value),
                                    int.Parse(rowMatch.Groups[4].Value)
                                )
                            )
                        );
                    }
                }
            }

            return (lines, new Point(maxX, maxY));
        }
    }
}