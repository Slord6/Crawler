using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.PageScoring
{
    /// <summary>
    /// Scores the comparison based on the first string containing any of the search terms
    /// </summary>
    class SearchTermsScorer : ZeroedStringComparisonScorer
    {
        private string[] searchTerms;

        public SearchTermsScorer(string[] searchTerms)
        {
            this.searchTerms = searchTerms;
        }

        /// <summary>
        /// Scores the input (disregarding the root) based on how many of the searchTerms are present in the text
        /// </summary>
        /// <param name="input">The text to be tested agains</param>
        /// <param name="root">Ignored</param>
        /// <returns></returns>
        public override double Score(string input, string root)
        {
            double score = 0;
            for (int i = 0; i < searchTerms.Length; i++)
            {
                if (input.Contains(searchTerms[i])) score++;
            }
            return score / (double)searchTerms.Length;
        }
    }
}
