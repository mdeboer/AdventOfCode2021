using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode2021.Exception;

namespace AdventOfCode2021.Day1
{
    public class Day1 : IPuzzle
    {
        public async Task<IEnumerable<string>> SolveAsync(CancellationToken cancellationToken = default)
        {
            int[] readings = await ReadInput("input.txt", cancellationToken);

            if (readings.Length < 3)
                throw new PuzzleInputException("Not enough readings to do calculation.");

            return new[]
            {
                await Part1(readings, cancellationToken),
                await Part2(readings, cancellationToken)
            };
        }

        private async Task<string> Part1(int[] readings, CancellationToken cancellationToken)
        {
            int timesIncreased = 0;
            int lastReading = -1;

            foreach (int reading in readings)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                if (lastReading >= 0 && reading > lastReading)
                    timesIncreased++;

                lastReading = reading;
            }

            return timesIncreased.ToString();
        }

        private async Task<string> Part2(int[] readings, CancellationToken cancellationToken)
        {
            int timesIncreased = 0;
            int currentPos = 0;
            int lastSum = -1;

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

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

            try
            {
                return Array.ConvertAll(rawReadings, int.Parse);
            }
            catch (System.Exception)
            {
                throw new PuzzleInputException();
            }
        }
    }
}