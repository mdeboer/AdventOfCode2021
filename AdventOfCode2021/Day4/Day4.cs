using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode2021.Exception;

namespace AdventOfCode2021.Day4
{
    public partial class Day4 : IPuzzle
    {
        public async Task<(IEnumerable<string>, TimeSpan)> SolveAsync(CancellationToken cancellationToken = default)
        {
            (int[] randomNumbers, IList<Board> boards) =
                await ReadInput(Path.Combine(AppContext.BaseDirectory, "Day4", "input.txt"), cancellationToken);

            Stopwatch sw = Stopwatch.StartNew();

            string[] results =
            {
                Part1(randomNumbers, boards, cancellationToken),
                Part2(randomNumbers, boards, cancellationToken)
            };

            sw.Stop();

            return (results, sw.Elapsed);
        }

        private string Part1(IEnumerable<int> randomNumbers, IList<Board> boards, CancellationToken cancellationToken)
        {
            ParallelOptions po = new()
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            foreach (int draw in randomNumbers)
            {
                ParallelLoopResult r = Parallel.For(0, boards.Count, po, (i, state) =>
                {
                    // Stop immediately if earlier board has already won.
                    if (state.ShouldExitCurrentIteration && state.LowestBreakIteration < i)
                        return;

                    // Mark the number on the board.
                    boards[i].Mark(draw);

                    // Check if board has bingo.
                    if (boards[i].HasBingo)
                        state.Break();
                });

                // Check if we have a winning board (the first).
                if (r.LowestBreakIteration != null)
                    return (boards[(int)r.LowestBreakIteration].SumUnmarked() * draw).ToString();
            }

            throw new System.Exception("No winning board!");
        }

        private string Part2(IEnumerable<int> randomNumbers, IList<Board> boards, CancellationToken cancellationToken)
        {
            ParallelOptions po = new()
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            object mutex = new();
            HashSet<int> winningBoards = new();
            List<int> winningDraws = new();

            foreach (int draw in randomNumbers)
                Parallel.For(0, boards.Count, po, i =>
                {
                    if (boards[i].HasBingo)
                        return;

                    // Mark the number on the board.
                    boards[i].Mark(draw);

                    // Check if board has bingo.
                    if (boards[i].HasBingo == false)
                        return;

                    lock (mutex)
                    {
                        if (winningBoards.Add(i))
                            winningDraws.Add(draw);
                    }
                });

            return (boards[winningBoards.Last()].SumUnmarked() * winningDraws.Last()).ToString();
        }

        /// <summary>
        ///     Read boards and random numbers to draw from input file.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="PuzzleInputException"></exception>
        private async Task<(int[], IList<Board>)> ReadInput(string file, CancellationToken cancellationToken = default)
        {
            await using FileStream fs = File.OpenRead(file);
            using StreamReader sr = new(fs, Encoding.ASCII);

            if (sr.EndOfStream)
                throw new PuzzleInputException();

            // Read first line with random numbers to be drawn.
            int[] randomNumbers = Array.ConvertAll((await sr.ReadLineAsync())!.Split(','), int.Parse);

            if (randomNumbers.Length == 0)
                throw new PuzzleInputException();

            // Read all boards
            List<Board> boards = new();
            List<string> currentBoard = new(5);

            while (sr.EndOfStream == false && cancellationToken.IsCancellationRequested == false)
            {
                string? line = await sr.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentBoard.Count == 0)
                        continue;

                    boards.Add(new Board(currentBoard.ToArray()));
                    currentBoard = new List<string>(5);

                    continue;
                }

                currentBoard.Add(line);
            }

            boards.Add(new Board(currentBoard.ToArray()));

            return (randomNumbers, boards.ToImmutableList());
        }
    }
}