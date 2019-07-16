using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.PageScoring
{
    /// <summary>
    /// Same as JaccardScorer but always scores against a fixed string
    /// </summary>
    class FixedComparisonJaccardScorer : JaccardScorer
    {
        private string fixedComparisonString;
        public FixedComparisonJaccardScorer(string fixedComparisonString)
        {
            this.fixedComparisonString = fixedComparisonString;
        }

        /// <summary>
        /// Calls base.Score, discarding root and replacing with the fixed string value
        /// </summary>
        /// <param name="input">The first value</param>
        /// <param name="root">Ignored, replaced with the fixed string</param>
        /// <returns>Jaccard Index score</returns>
        public override double Score(string input, string root)
        {
            return base.Score(input, fixedComparisonString);
        }
    }
}
