using System;
using System.Collections.Generic;
using System.Linq;
using Crawler.Internet;
using Crawler.Crawling;
using System.IO;
using Crawler.Crawling.Interfaces;
using Crawler.Crashes;
using Crawler.PageScoring;
using Crawler.PageScoring.ThresholdRules;

namespace Crawler
{
    class Program
    {
        private static List<ICrawlDatabase> databases = new List<ICrawlDatabase>();
        private static Uri[] seeds = new Uri[] {};

        static void Main(string[] args)
        {
            if (!HandleArgs(args)) return;

            CrawlWorker crawler = null;

            try
            {
                
                crawler = new CrawlWorker(seeds);

                Run(crawler);
            }
            catch (Exception ex)
            {
                CrashManager.Handle(ex, new object[] { crawler });
            }

            Console.ReadKey();
        }

        static void Run(CrawlWorker crawler)
        {
            CandidateTracker candidateTracker = new CandidateTracker();
            ScoreManager scoreManager = new ScoreManager(
                new List<IStringComparisonScorer> {
                    new JaccardScorer(),
                    //new FixedComparisonJaccardScorer(File.ReadAllText(@".\comparisonText.txt"))
                    new SearchTermsScorer(File.ReadAllText(@".\comparisonText.txt").Split(' '))
                },
                new List<IScoreThresholdRule> {
                    new MinMaxScoreThresholdRule(0.4, 0.99, candidateTracker.HandleCandidate)
                }
                );
            string frontierFileName = "Frontier_" + CrawlSettings.CrawlName;
            foreach (PageCrawl crawl in crawler.Start())
            {
                databases.ForEach(db => db.InsertPageCrawl(crawl));

                scoreManager.Score(crawl, crawl.Page.LinkedFrom?.Crawl?.Content);
                candidateTracker.WriteToDisk();

                File.WriteAllText(frontierFileName, crawler.Frontiers);
            }

            Console.WriteLine("Ran out of links!");
        }

        /// <summary>
        /// Handle passed command line arguments
        /// </summary>
        /// <param name="args">The passed arguments</param>
        /// <returns>True if the program should continue, false otherwise</returns>
        private static bool HandleArgs(string[] args)
        {
            if (args.Length < 2)
            {
                PrintHelp();
                return false;
            }
            foreach(char invalidChar in Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()))
            {
                if(args[0].Contains(invalidChar))
                {
                    Console.WriteLine("Search name must be usable as file/directory name");
                    return false;
                }
            }

            CrawlSettings.CrawlName = args[0];
            databases.Add(new LoggerDatabase());
            databases.Add(new FlatFileDatabase(CrawlSettings.CrawlName));
            Console.WriteLine("Search: " + CrawlSettings.CrawlName);

            Browser.UserAgentContactInformation = args[1];
            Console.WriteLine("User Agent: " + Browser.UserAgent);
            // Rest are seeds
            if (!ParseSeeds(args, 2)) return false;
            Console.WriteLine();
            return true;
        }

        /// <summary>
        /// Parse the seeds for this search
        /// </summary>
        /// <param name="args"></param>
        /// <param name="firstSeed"></param>
        /// <returns>Were the seeds parsed successfully</returns>
        private static bool ParseSeeds(string[] seedInputs, int firstSeed, List<Uri> uris = null)
        {
            Console.WriteLine("Adding " + (seedInputs.Length - firstSeed) + " seeds...");
            if (uris == null) uris = new List<Uri>();
            for (int i = firstSeed; i < seedInputs.Length; i++)
            {
                string input = seedInputs[i];
                try
                {
                    if (File.Exists(input)) // input is file with list of seeds
                    {
                        ParseSeeds(File.ReadAllLines(input), 0, uris);
                        continue;
                    }
                    else // input is url
                    {
                        uris.Add(new Uri(input));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(input + ", is probably invalid");
                    PrintHelp();
                    return false;
                }
            }
            seeds = uris.ToArray();
            return true;
        }

        private static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine(" =: CRAWLER HELP :=");
            Console.WriteLine("Argument 1:");
            Console.WriteLine("\tUnique name for this search, used for file names etc");
            Console.WriteLine("Argument 2:");
            Console.WriteLine("\tContact details, in case a bug causes problems for others");
            Console.WriteLine("Argument 3->N");
            Console.WriteLine("\tAll follwing arguments are url seeds eg. https://reddit.com AND/OR path to files with a seed per line");
            Console.WriteLine();
        }
    }
}
