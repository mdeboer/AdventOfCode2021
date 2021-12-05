using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode2021.Exception;

namespace AdventOfCode2021.Day3
{
    public class Day3 : IPuzzle
    {
        public async Task<IEnumerable<string>> SolveAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<string> readings = (await File.ReadAllLinesAsync(
                Path.Combine(AppContext.BaseDirectory, "Day3", "input.txt"),
                cancellationToken
            )).ToImmutableList();

            if (readings.Count == 0)
                throw new PuzzleInputException();

            return new[]
            {
                Part1(readings, cancellationToken),
                Part2(readings, cancellationToken)
            };
        }

        private string Part1(IReadOnlyList<string> diagnosticReport, CancellationToken cancellationToken)
        {
            // Get pivoted results
            string[] pivotedReadings = PivotArray(diagnosticReport, cancellationToken);

            // Strings holding the binary values for gamma and epsilon.
            string gamma = "";
            string epsilon = "";

            foreach (string reading in pivotedReadings)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Count the number of positive and negative bits.
                int numPositiveBits = Regex.Matches(reading, "1").Count;
                int numNegativeBits = reading.Length - numPositiveBits;

                // Most dominant bit is 0.
                gamma += numPositiveBits > numNegativeBits ? "1" : "0";
                epsilon += numNegativeBits > numPositiveBits ? "1" : "0";
            }

            if (gamma.Length == 0 || epsilon.Length == 0)
                throw new System.Exception("We did an oops.");

            // Convert to integer and multiply them.
            return (Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2)).ToString();
        }

        private string Part2(IReadOnlyList<string> diagnosticReport, CancellationToken cancellationToken)
        {
            int oxygen = CalculateLifeSupportRating(diagnosticReport, true, cancellationToken);
            int co2 = CalculateLifeSupportRating(diagnosticReport, false, cancellationToken);

            return (oxygen * co2).ToString();
        }

        /// <summary>
        ///     Pivot an array, turning values from a column into a row. For example all values of column 0 from every row
        ///     concatted becomes the first row and so on.
        /// </summary>
        /// <param name="diagnosticReport"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Array with the columns and rows pivotted.</returns>
        private string[] PivotArray(
            IReadOnlyList<string> diagnosticReport,
            CancellationToken cancellationToken = default
        )
        {
            // Get the length of the first line of the report.
            int numColumns = diagnosticReport[0].Length;

            // Create an array of <length> strings to fill later.
            string[] swappedReadings = Enumerable.Repeat("", numColumns).ToArray();

            // Swap columns to rows.
            foreach (string reading in diagnosticReport)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Read every character (column) of the row and append its value to the correct result row.
                for (int i = 0; i < numColumns; i++)
                    swappedReadings[i] += reading[i];
            }

            foreach (string swappedReading in swappedReadings)
                if (Regex.IsMatch(swappedReading, "(0|1)+") == false)
                    throw new PuzzleInputException();

            return swappedReadings;
        }

        /// <summary>
        ///     Calculate a part of the life support rating, either for the oxygen generator or the CO2 scrubber.
        /// </summary>
        /// <param name="diagnosticReport"></param>
        /// <param name="oxygen">
        ///     If true, calculate the rating for the oxygen generator. Otherwise calculate the rating for the CO2
        ///     scrubber.
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns>Life support rating as integer.</returns>
        private int CalculateLifeSupportRating(
            IReadOnlyList<string> diagnosticReport,
            bool oxygen,
            CancellationToken cancellationToken = default
        )
        {
            // Get pivoted input.
            string[] pivotResults = PivotArray(diagnosticReport, cancellationToken);

            // Working set of results.
            List<string> results = diagnosticReport.ToList();

            // Current column position.
            int position = 0;

            // Reduce results until we have one value left.
            while (results.Count > 1)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Count the number of positive and negative bits.
                int numPositiveBits = Regex.Matches(pivotResults[position], "1").Count;
                int numNegativeBits = pivotResults[position].Length - numPositiveBits;

                // Boolean indicating which bit value to keep.
                bool valueToKeep =
                    oxygen && numPositiveBits > numNegativeBits
                    || oxygen == false && numPositiveBits < numNegativeBits
                    || numPositiveBits == numNegativeBits && oxygen;

                // Reduce results by keeping only the elements with the correct value at the current column position.
                results = results.FindAll(r => r[position] == '1' == valueToKeep);

                // Stop if we recuded the results to a single value.
                if (results.Count == 1)
                    break;

                // Pivot reduced results.
                pivotResults = PivotArray(results, cancellationToken);

                // Advance column position for the reduced results in the next iteration.
                position++;

                if (position >= pivotResults.Length)
                    throw new System.Exception("We did an oops.");
            }

            return Convert.ToInt32(results.First(), 2);
        }
    }
}