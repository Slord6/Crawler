using System;
using System.Collections.Generic;
using System.Linq;

namespace Crawler.PageScoring
{
    /// <summary>
    /// Scores strings based on the JaccardIndex
    /// </summary>
    class JaccardScorer : ZeroedStringComparisonScorer
    {
        /// <summary>
        /// Scores based on the Jaccard Index - https://en.wikipedia.org/wiki/Jaccard_index
        /// Score is the size of the intersection divided by the size of the union of the unique values in the strings when separated by the space character
        /// </summary>
        /// <param name="input">The first string</param>
        /// <param name="root">The second string</param>
        /// <returns>The Jaccard Index score of the two strings</returns>
        public override double Score(string input, string root)
        {
            if (input == null || root == null) return 0;
            HashSet<string> uniqueFromRoot = this.ToUniqueValues(root);
            HashSet<string> uniqueFromInput = this.ToUniqueValues(input);
            double unionSize = uniqueFromInput.Where(x => uniqueFromRoot.Contains(x)).Count();

            return unionSize / (double)(uniqueFromRoot.Count + uniqueFromInput.Count);
        }

        private HashSet<string> ToUniqueValues(string text)
        {
            HashSet<string> uniqueValues = new HashSet<string>();
            foreach (string rootVal in text.Split(' '))
            {
                if (!uniqueValues.Contains(rootVal)) uniqueValues.Add(rootVal);
            }
            return uniqueValues;
        }
    }
}
