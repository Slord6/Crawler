using Crawler.Crawling;
using Crawler.PageScoring.ThresholdRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.PageScoring
{
    class ScoreManager
    {
        private List<IStringComparisonScorer> scorers;
        private List<IScoreThresholdRule> thresholdActions;

        public ScoreManager(List<IStringComparisonScorer> scorers, List<IScoreThresholdRule> thresholdActions)
        {
            this.scorers = scorers;
            this.thresholdActions = thresholdActions;
        }

        /// <summary>
        /// Score a page using the average of all scores
        /// Trigger rules with the score and crawl
        /// </summary>
        /// <param name="crawl">The crawl to score</param>
        public void Score(PageCrawl crawl, string comparison)
        {
            double score = 0;
            foreach (IStringComparisonScorer scorePipe in scorers)
            {
                score += scorePipe.Score(crawl.Content, comparison);
            }
            score = score / (double)scorers.Count;
            Console.WriteLine(crawl.Page + " scored " + score);

            foreach (IScoreThresholdRule rule in thresholdActions)
            {
                rule.Trigger(score, crawl);
            }
        }
    }
}
