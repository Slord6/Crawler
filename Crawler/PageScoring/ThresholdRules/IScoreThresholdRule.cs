using Crawler.Crawling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.PageScoring.ThresholdRules
{
    interface IScoreThresholdRule
    {
        /// <summary>
        /// Triggers an action if the score and/or crawl meet some threshold
        /// </summary>
        /// <param name="score"></param>
        /// <param name="crawl"></param>
        void Trigger(double score, PageCrawl crawl);
    }
}
