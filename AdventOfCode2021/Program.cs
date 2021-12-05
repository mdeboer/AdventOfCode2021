using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // List of days / puzzles to solve.
            Dictionary<int, Task<IEnumerable<string>>> puzzles = new()
            {
                { 1, new Day1.Day1().SolveAsync() },
                { 2, new Day2.Day2().SolveAsync() }
            };

            // Wait untill all puzzles are solved.
            Task.WaitAll(puzzles.Values.ToArray());

            // Output the results
            Console.WriteLine("Advent of Code 2021");
            Console.WriteLine("by Maarten de Boer <maarten@cloudstek.nl>");
            Console.WriteLine();

            foreach (KeyValuePair<int, Task<IEnumerable<string>>> puzzle in puzzles)
            {
                string results = string.Join(", ", puzzle.Value.Result);

                Console.WriteLine(
                    $"Day {puzzle.Key.ToString().PadLeft(2, '0')}: {results}"
                );
            }
        }
    }
}