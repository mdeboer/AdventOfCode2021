using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    public interface IPuzzle
    {
        /// <summary>
        ///     Solve puzzle and return a list of answers (one for every part).
        /// </summary>
        /// <returns>Answers for all parts and how long it took.</returns>
        public Task<(IEnumerable<string>, TimeSpan)> SolveAsync(CancellationToken cancellationToken = default);
    }
}