using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.PageScoring
{
    interface IStringComparisonScorer
    {
        /// <summary>
        /// Score the input based on some metric incorpoarting the root
        /// </summary>
        /// <param name="input">The string to score</param>
        /// <param name="root">The string to score against</param>
        /// <returns>Some score between 0 and 1</returns>
        double Score(string input, string root);
    }
}
