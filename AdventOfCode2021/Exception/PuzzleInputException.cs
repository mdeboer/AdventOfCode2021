namespace AdventOfCode2021.Exception
{
    public class PuzzleInputException : System.Exception
    {
        public PuzzleInputException(
            string? message = "This puzzle received invalid or unexpected input."
        ) : base(message)
        {
        }
    }
}