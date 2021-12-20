using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode2021.Exception;

namespace AdventOfCode2021.Day1
{
    public class Day1 : IPuzzle
    {
        public async Task<(IEnumerable<string>, TimeSpan)> SolveAsync(CancellationToken cancellationToken = default)
        {
            int[] readings = await ReadInput("input.txt", cancellationToken);

            Stopwatch sw = Stopwatch.StartNew();

            string[] results =
            {
                Part1(readings, cancellationToken),
                Part2(readings, cancellationToken)
            };

            sw.Stop();

            return (results, sw.Elapsed);
        }

        private string Part1(IReadOnlyCollection<int> readings, CancellationToken cancellationToken)
        {
            if (readings.Count < 2)
                throw new PuzzleInputException("Not enough readings to do calculation.");

            int timesIncreased = 0;
            int lastReading = -1;

            foreach (int reading in readings)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (lastReading >= 0 && reading > lastReading)
                    timesIncreased++;

                lastReading = reading;
            }

            return timesIncreased.ToString();
        }

        private string Part2(IReadOnlyCollection<int> readings, CancellationToken cancellationToken)
        {
            if (readings.Count < 3)
                throw new PuzzleInputException("Not enough readings to do calculation.");

            int timesIncreased = 0;
            int currentPos = 0;
            int lastSum = -1;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                int[] window = readings.Skip(currentPos).Take(3).ToArray();

                // Skip if we haven't got enough readings left for another window.
                if (window.Length < 3)
                    break;

                int windowSum = window.Sum();

                if (lastSum >= 0 && windowSum > lastSum)
                    timesIncreased++;

                lastSum = windowSum;

                currentPos++;
            }

            return timesIncreased.ToString();
        }

        /// <summary>
        ///     Read input file (one number per line).
        /// </summary>
        /// <param name="file">File name (not including path).</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Integer value of each line</returns>
        /// <exception cref="PuzzleInputException"></exception>
        private async Task<int[]> ReadInput(string file, CancellationToken cancellationToken = default)
        {
            string[] rawReadings = await File.ReadAllLinesAsync(
                Path.Combine(AppContext.BaseDirectory, "Day1", file),
                cancellationToken
            );

            return Array.ConvertAll(rawReadings, int.Parse);
        }
    }
}