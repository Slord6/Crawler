using Crawler.Crawling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.PageScoring
{
    /// <summary>
    /// Keeps track of candidates and scores that are passed 
    /// </summary>
    class CandidateTracker
    {
        private HashSet<CandidateScore> candidates;

        public CandidateTracker()
        {
            candidates = new HashSet<CandidateScore>();
        }

        /// <summary>
        /// Handle a new candidate
        /// Keeps a reference to the crawl and score, if the page is already present in the store then update score to the maximum of both
        /// </summary>
        /// <param name="score">The score of the candidate</param>
        /// <param name="candiate">The candidate</param>
        public void HandleCandidate(double score, PageCrawl candiate)
        {
            // Update existing
            IEnumerable<CandidateScore> matchingCandidates = candidates.Where(x => x.Candidate.Page.ToString() == candiate.Page.ToString());
            if (matchingCandidates.Count() > 0)
            {
                CandidateScore existing = matchingCandidates.First();
                existing.Score = Math.Max(existing.Score, score);
                Console.WriteLine("Existing candidate: " + candiate.Page + " - score updated to " + existing.Score);
                return;
            }

            // Add new
            Console.WriteLine("New candidate: " + candiate.Page + " - " + score);
            candidates.Add(new CandidateScore(score, candiate));
        }

        /// <summary>
        /// Compile a summary of all candidates and save to [crawl name]_candidates.txt
        /// </summary>
        public void WriteToDisk()
        {
            StringBuilder summary = new StringBuilder();

            foreach (CandidateScore candidateScore in candidates.OrderByDescending(x => x.Score))
            {
                summary.AppendLine(candidateScore.Candidate.Page + " <> " + candidateScore.Score);
            }

            File.WriteAllText(CrawlSettings.CrawlName + "_candidates.txt", summary.ToString());
        }

        /// <summary>
        /// Helper class to bundle a candidate and its score
        /// </summary>
        private class CandidateScore
        {
            private double score;
            private PageCrawl candidate;
            public double Score
            {
                get
                {
                    return score;
                }
                set
                {
                    this.score = value;
                }
            }
            public PageCrawl Candidate
            {
                get
                {
                    return candidate;
                }
            }
            public CandidateScore(double score, PageCrawl candidate)
            {
                this.score = score;
                this.candidate = candidate;
            }
        }
    }
}
