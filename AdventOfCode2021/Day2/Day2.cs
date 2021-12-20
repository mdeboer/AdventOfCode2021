using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode2021.Exception;

namespace AdventOfCode2021.Day2
{
    public class Day2 : IPuzzle
    {
        public async Task<(IEnumerable<string>, TimeSpan)> SolveAsync(CancellationToken cancellationToken = default)
        {
            List<(Direction, int)> commands = await ReadInput(
                Path.Combine(AppContext.BaseDirectory, "Day2", "input.txt"),
                cancellationToken);

            Stopwatch sw = Stopwatch.StartNew();

            string[] results =
            {
                Part1(commands, cancellationToken),
                Part2(commands, cancellationToken)
            };

            sw.Stop();

            return (results, sw.Elapsed);
        }

        private string Part1(IEnumerable<(Direction, int)> commands, CancellationToken cancellationToken)
        {
            int x = 0;
            int y = 0;

            foreach ((Direction direction, int steps) in commands)
            {
                cancellationToken.ThrowIfCancellationRequested();

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

        private string Part2(IEnumerable<(Direction, int)> commands, CancellationToken cancellationToken)
        {
            int x = 0;
            int y = 0;
            int aim = 0;

            foreach ((Direction direction, int steps) in commands)
            {
                cancellationToken.ThrowIfCancellationRequested();

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
                        y += aim * steps;
                        break;
                }
            }

            if (x <= 0 || y <= 0)
                throw new PuzzleInputException();

            return (x * y).ToString();
        }

        /// <summary>
        ///     Read commands from input file.
        /// </summary>
        /// <param name="file">File name (not including path).</param>
        /// <param name="cancellationToken"></param>
        /// <returns>List of direction -> steps tuples.</returns>
        /// <exception cref="PuzzleInputException"></exception>
        private async Task<List<(Direction, int)>> ReadInput(string file, CancellationToken cancellationToken = default)
        {
            List<(Direction, int)> readings = new();

            Regex commandRegex = new("^(up|down|forward) ([0-9]+)", RegexOptions.Compiled);

            await using (FileStream fs = File.OpenRead(file))
            {
                using (StreamReader sr = new(fs, Encoding.ASCII))
                {
                    while (sr.EndOfStream == false && cancellationToken.IsCancellationRequested == false)
                    {
                        string? line = sr.ReadLine();

                        if (line == null)
                            throw new PuzzleInputException();

                        Match command = commandRegex.Match(line);

                        if (command.Success == false)
                            throw new PuzzleInputException();

                        int steps = int.Parse(command.Groups[2].Value);

                        switch (command.Groups[1].Value)
                        {
                            case "up":
                                readings.Add((Direction.Up, steps));
                                break;
                            case "down":
                                readings.Add((Direction.Down, steps));
                                break;
                            case "forward":
                                readings.Add((Direction.Forward, steps));
                                break;
                            default:
                                throw new PuzzleInputException("Invalid direction in input.");
                        }
                    }
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