using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode2021.Exception;

namespace AdventOfCode2021.Day2
{
    public class Day2 : IPuzzle
    {
        public async Task<IEnumerable<string>> SolveAsync(CancellationToken cancellationToken = default)
        {
            List<(Direction, int)> readings = await ReadInput("input.txt", cancellationToken);

            return new[]
            {
                Part1(readings, cancellationToken),
                Part2(readings, cancellationToken)
            };
        }

        private string Part1(List<(Direction, int)> readings, CancellationToken cancellationToken)
        {
            int x = 0;
            int y = 0;

            foreach ((Direction direction, int steps) in readings)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                switch (direction)
                {
                    case Direction.Up:
                        y -= steps;
                        break;
                    case Direction.Down:
                        y += steps;
                        break;
                    case Direction.Forward:
                        x += steps;
                        break;
                }
            }

            if (x <= 0 || y <= 0)
                throw new PuzzleInputException();

            return (x * y).ToString();
        }

        private string Part2(List<(Direction, int)> readings, CancellationToken cancellationToken)
        {
            int x = 0;
            int y = 0;
            int aim = 0;

            foreach ((Direction direction, int steps) in readings)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                switch (direction)
                {
                    case Direction.Up:
                        aim -= steps;
                        break;
                    case Direction.Down:
                        aim += steps;
                        break;
                    case Direction.Forward:
                        x += steps;
                        y += (aim * steps);
                        break;
                }
            }

            if (x <= 0 || y <= 0)
                throw new PuzzleInputException();

            return (x * y).ToString();
        }

        /// <summary>
        ///     Read input file (direction and steps)
        /// </summary>
        /// <param name="file">File name (not including path).</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of direction -> steps tuples.</returns>
        /// <exception cref="PuzzleInputException"></exception>
        private async Task<List<(Direction, int)>> ReadInput(string file, CancellationToken cancellationToken = default)
        {
            string[] rawReadings = await File.ReadAllLinesAsync(
                Path.Combine(AppContext.BaseDirectory, "Day2", file),
                cancellationToken
            );

            List<(Direction, int)> readings = new();

            foreach (string reading in rawReadings)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                string[] splitReading = reading.Split(' ', 2);

                if (splitReading.Length != 2)
                    throw new PuzzleInputException();

                switch (splitReading[0].ToLower())
                {
                    case "up":
                        readings.Add((Direction.Up, int.Parse(splitReading[1])));
                        break;
                    case "down":
                        readings.Add((Direction.Down, int.Parse(splitReading[1])));
                        break;
                    case "forward":
                        readings.Add((Direction.Forward, int.Parse(splitReading[1])));
                        break;
                    default:
                        throw new PuzzleInputException("Invalid direction in input.");
                }
            }

            return readings;
        }

        private enum Direction
        {
            Up,
            Down,
            Forward
        }
    }
}