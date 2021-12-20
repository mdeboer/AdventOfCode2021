using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day7
{
    public class Day7 : IPuzzle
    {
        public async Task<(IEnumerable<string>, TimeSpan)> SolveAsync(CancellationToken cancellationToken = default)
        {
            int[] positions = ReadInput(Path.Combine(AppContext.BaseDirectory, "Day7", "input.txt"));

            Stopwatch sw = Stopwatch.StartNew();

            string[] results =
            {
                Part1(positions, cancellationToken),
                Part2(positions, cancellationToken)
            };

            sw.Stop();

            return (results, sw.Elapsed);
        }

        public string Part1(int[] positions, CancellationToken cancellationToken)
        {
            int median = positions.Median();

            int fuel = 0;

            foreach (int position in positions)
                fuel += Math.Abs(position - median);

            return fuel.ToString();
        }

        public string Part2(int[] positions, CancellationToken cancellationToken)
        {
            double average = positions.Average();

            int flooredFuel = 0;
            int ceiledFuel = 0;

            foreach (int position in positions)
            {
                // Calculate the number of steps from the ceiled or floored average as we need a round number.
                int flooredSteps = Math.Abs(position - (int)Math.Floor(average));
                int ceiledSteps = Math.Abs(position - (int)Math.Ceiling(average));

                // Do the fuel calculations for each number of steps.
                flooredFuel += flooredSteps * (flooredSteps + 1) / 2;
                ceiledFuel += ceiledSteps * (ceiledSteps + 1) / 2;
            }

            // Return the result of whatever used the least fuel (floored or ceiled).
            return Math.Min(flooredFuel, ceiledFuel).ToString();
        }

        public int[] ReadInput(string file)
        {
            string[] input = File.ReadLines(file, Encoding.ASCII).First().Split(',');

            return Array.ConvertAll(input, int.Parse);
        }
    }
}