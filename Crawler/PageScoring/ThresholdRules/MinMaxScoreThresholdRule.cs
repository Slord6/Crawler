using Crawler.Crawling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.PageScoring.ThresholdRules
{
    class MinMaxScoreThresholdRule : IScoreThresholdRule
    {
        private double min;
        private double max;
        private Action<double, PageCrawl> action;

        public MinMaxScoreThresholdRule(double minThreshold, double maxThreshold, Action<double, PageCrawl> action)
        {
            this.min = minThreshold;
            this.max = maxThreshold;
            this.action = action;
        }

        /// <summary>
        /// Triggers the action if the score falls between the min and max thresholds
        /// </summary>
        /// <param name="score">The score to test</param>
        /// <param name="crawl">The crawl the scroe is for</param>
        public void Trigger(double score, PageCrawl crawl)
        {
            if (score > min && score < max) action.Invoke(score, crawl);
        }
    }
}
