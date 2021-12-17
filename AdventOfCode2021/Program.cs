using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // List of days / puzzles to solve.
            List<Task<(IEnumerable<string>, TimeSpan)>> puzzles = new()
            {
                new Day1.Day1().SolveAsync(),
                new Day2.Day2().SolveAsync(),
                new Day3.Day3().SolveAsync(),
                new Day4.Day4().SolveAsync()
            };

            // Wait untill all puzzles are solved.
            Task.WaitAll(puzzles.ToArray());

            // Output the results
            Console.WriteLine("Advent of Code 2021");
            Console.WriteLine("by Maarten de Boer <maarten@cloudstek.nl>");
            Console.WriteLine();

            for (int i = 1; i <= puzzles.Count; i++)
            {
                (IEnumerable<string> results, TimeSpan elapsed) = puzzles[i - 1].Result;

                Console.WriteLine(
                    $"Day {i.ToString().PadLeft(2, '0')}: {string.Join(", ", results)} ({elapsed.TotalMilliseconds}ms)"
                );
            }
        }
    }
}