using Crawler.Crawling;
using Crawler.PageScoring.ThresholdRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.PageScoring.ThresholdRules
{
    class ScoreThresholdRule : IScoreThresholdRule
    {
        private double threshold;
        private Action<double, PageCrawl> action;

        public ScoreThresholdRule(double threshold, Action<double, PageCrawl> action)
        {
            this.threshold = threshold;
            this.action = action;
        }

        /// <summary>
        /// Triggers the action of the rule if the score is greater than or equal to the threshold
        /// </summary>
        /// <param name="score">The score to test against the threshold</param>
        public void Trigger(double score, PageCrawl crawl)
        {
            if (score >= threshold) action.Invoke(score, crawl);
        }
    }
}
