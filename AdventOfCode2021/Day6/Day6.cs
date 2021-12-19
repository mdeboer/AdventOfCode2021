using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode2021.Exception;

namespace AdventOfCode2021.Day6
{
    public class Day6 : IPuzzle
    {
        public async Task<(IEnumerable<string>, TimeSpan)> SolveAsync(CancellationToken cancellationToken = default)
        {
            // All lines from input.
            IEnumerable<long> fish = ReadInput(Path.Combine(AppContext.BaseDirectory, "Day6", "input.txt"));

            Stopwatch sw = Stopwatch.StartNew();

            string[] results =
            {
                Part1(fish, cancellationToken),
                Part2(fish, cancellationToken)
            };

            sw.Stop();

            return (results, sw.Elapsed);
        }

        private string Part1(IEnumerable<long> initialFish, CancellationToken cancellationToken) =>
            SimulateFish(initialFish, 80, cancellationToken).ToString();

        private string Part2(IEnumerable<long> initialFish, CancellationToken cancellationToken) =>
            SimulateFish(initialFish, 256, cancellationToken).ToString();

        private long SimulateFish(IEnumerable<long> initialFish, int days, CancellationToken cancellationToken)
        {
            if (days <= 0)
                throw new ArgumentOutOfRangeException(nameof(days));

            long[] fish = initialFish.ToArray();

            for (int day = 1; day <= days; day++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Get number of fish that will spawn a new fish today
                long newFish = fish[0];

                // Shift the array left, decrementing the timer for all fish
                Array.Copy(fish, 1, fish, 0, 8);

                // Reset the fishes that spawned a new fish to a timer of 6
                fish[6] += newFish;

                // Add the new fish
                fish[8] = newFish;
            }

            return fish.Sum();
        }

        private IEnumerable<long> ReadInput(string file)
        {
            // Array with a sum of the fish with a certain timer value as index.
            long[] result = new long[9];

            using FileStream fs = File.OpenRead(file);
            using StreamReader sr = new(fs, Encoding.ASCII);

            string? line = sr.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
                throw new PuzzleInputException();

            foreach (int i in Array.ConvertAll(line.Split(','), int.Parse))
            {
                if (i is < 0 or > 8)
                    throw new PuzzleInputException();

                result[i]++;
            }

            return result;
        }
    }
}