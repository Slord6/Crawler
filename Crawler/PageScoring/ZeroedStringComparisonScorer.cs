using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.PageScoring
{
    /// <summary>
    /// Abstract comparison scorer
    /// Acts as parent to other scorers to implement shared functionality
    /// </summary>
    abstract class ZeroedStringComparisonScorer : IStringComparisonScorer
    {
        /// <summary>
        /// Always returns 0, as if the strings were completely different, irrelevant of content
        /// </summary>
        /// <param name="input">First string, ignored</param>
        /// <param name="root">Second string, ignores</param>
        /// <returns>Zero</returns>
        public virtual double Score(string input, string root)
        {
            return 0;
        }
    }
}
