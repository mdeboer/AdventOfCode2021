using System;
using AdventOfCode2021.Exception;

namespace AdventOfCode2021.Day4
{
    public partial class Day4
    {
        private class Board
        {
            private readonly int _rows;
            private readonly int _cols;

            private readonly bool[,] _marks;
            private readonly int[,] _numbers;

            private bool _bingo;

            public Board(string[] lines, int rows = 5, int cols = 5)
            {
                if (lines.Length != rows)
                    throw new ArgumentOutOfRangeException(nameof(lines));

                _rows = rows;
                _cols = cols;

                // Create 5x5 grid of marks
                _marks = new bool[rows, cols];

                for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    _marks[i, j] = false;

                // Parse rows.
                _numbers = new int[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    // Split row by spaces, trim/remove empty entries and convert to integers.
                    int[] values = Array.ConvertAll(
                        lines[i].Split(
                            ' ',
                            5,
                            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
                        ),
                        int.Parse
                    );

                    if (values.Length != cols)
                        throw new PuzzleInputException();

                    for (int j = 0; j < cols; j++)
                        _numbers[i, j] = values[j];
                }
            }

            /// <summary>
            ///     Marks the number on the board (if on the board).
            /// </summary>
            /// <param name="number">The number that was drawn to mark on the board.</param>
            /// <returns>Boolean indicating if the number was on the board.</returns>
            public bool Mark(int number)
            {
                for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _cols; j++)
                    if (_numbers[i, j] == number)
                    {
                        _marks[i, j] = true;
                        return true;
                    }

                return false;
            }

            /// <summary>
            ///     Sum all unmarked numbers.
            /// </summary>
            /// <returns>The sum of all unmarked numbers on the board.</returns>
            public int SumUnmarked()
            {
                int sum = 0;

                for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _cols; j++)
                {
                    if (_marks[i, j])
                        continue;

                    sum += _numbers[i, j];
                }

                return sum;
            }

            /// <summary>
            ///     Check if the board has bingo!
            /// </summary>
            public bool HasBingo
            {
                get
                {
                    if (_bingo)
                        return true;

                    bool[] colsWon = { true, true, true, true, true };

                    for (int i = 0; i < _rows; i++)
                    {
                        bool rowWon = true;

                        for (int j = 0; j < _cols; j++)
                            // If one column in a row is false, this row is not a winner.
                            if (_marks[i, j] == false)
                            {
                                colsWon[j] = false;
                                rowWon = false;
                            }

                        // This row has all numbers, win!
                        if (rowWon)
                            return true;
                    }

                    // Check if any of the columns has all winning numbers.
                    return _bingo = Array.Exists(colsWon, colWon => colWon);
                }
            }
        }
    }
}