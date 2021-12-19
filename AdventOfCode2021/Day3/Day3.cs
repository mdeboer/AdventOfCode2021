using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode2021.Exception;

namespace AdventOfCode2021.Day3
{
    public class Day3 : IPuzzle
    {
        public async Task<(IEnumerable<string>, TimeSpan)> SolveAsync(CancellationToken cancellationToken = default)
        {
            BitArray[] readings = await ReadInput(
                Path.Combine(AppContext.BaseDirectory, "Day3", "input.txt"),
                cancellationToken
            );

            Stopwatch sw = Stopwatch.StartNew();

            string[] results =
            {
                Part1(readings, cancellationToken),
                Part2(readings, cancellationToken)
            };

            sw.Stop();

            return (results, sw.Elapsed);
        }

        private string Part1(BitArray[] diagnosticReport, CancellationToken cancellationToken)
        {
            int numColumns = diagnosticReport[0].Count;
            int numRows = diagnosticReport.Length;

            BitArray gamma = new(numColumns, false);
            BitArray epsilon = new(numColumns, false);

            for (int i = 0; i < numColumns; i++)
            {
                int numPositive = CountPositiveBitsInColumn(diagnosticReport, i, cancellationToken);

                if (numPositive > numRows / 2)
                    gamma[i] = true;

                epsilon[i] = gamma[i] == false;
            }

            // Convert to integer and multiply them.
            return (gamma.Reverse().ToInt() * epsilon.Reverse().ToInt()).ToString();
        }

        private string Part2(BitArray[] diagnosticReport, CancellationToken cancellationToken)
        {
            List<BitArray> oxygen = diagnosticReport.ToList();
            List<BitArray> co2 = diagnosticReport.ToList();

            int i = 0;

            while ((oxygen.Count > 1 || co2.Count > 1) && cancellationToken.IsCancellationRequested == false)
            {
                if (oxygen.Count > 1)
                {
                    bool keepOxygen = CountPositiveBitsInColumn(oxygen, i, cancellationToken) >=
                                      (double)oxygen.Count / 2;

                    oxygen.RemoveAll(r => r[i] != keepOxygen);
                }

                if (co2.Count > 1)
                {
                    bool keepCo2 = CountPositiveBitsInColumn(co2, i, cancellationToken) < (double)co2.Count / 2;

                    co2.RemoveAll(r => r[i] != keepCo2);
                }

                i++;
            }

            return (oxygen.First().Reverse().ToInt() * co2.First().Reverse().ToInt()).ToString();
        }

        /// <summary>
        ///     Count number of positive bits at a position of every BitArray in the list.
        /// </summary>
        /// <param name="arrays"></param>
        /// <param name="column"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private int CountPositiveBitsInColumn(
            IEnumerable<BitArray> arrays,
            int column,
            CancellationToken cancellationToken = default
        )
        {
            int result = 0;

            foreach (BitArray array in arrays)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (array[column])
                    result++;
            }

            return result;
        }

        /// <summary>
        ///     Read input into an array of BitArray.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Array of BitArray containing all readings</returns>
        /// <exception cref="PuzzleInputException"></exception>
        private async Task<BitArray[]> ReadInput(string file, CancellationToken cancellationToken)
        {
            int reportLength = File.ReadLines(file).Count();

            if (reportLength == 0)
                throw new PuzzleInputException();

            BitArray[] report = new BitArray[reportLength];

            await using (FileStream fs = File.OpenRead(file))
            {
                using (StreamReader sr = new(fs, Encoding.ASCII))
                {
                    int i = 0;

                    while (sr.EndOfStream == false && cancellationToken.IsCancellationRequested == false)
                    {
                        string? line = await sr.ReadLineAsync();

                        if (line == null)
                            continue;

                        report[i] = new BitArray(line.Length);

                        int x = 0;

                        foreach (char c in line)
                        {
                            report[i][x] = c == '1';
                            x++;
                        }

                        i++;
                    }
                }
            }

            return report;
        }
    }
}