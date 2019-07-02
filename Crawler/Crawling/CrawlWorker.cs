using Crawler.Internet;
using Crawler.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler.Crawling
{
    public class CrawlWorker
    {
        private Queue<Uri> frontier;
        private HashSet<string> visited;
        private Dictionary<string, Robots> robots;
        private PageCrawl latestCrawl;

        public CrawlWorker(Uri[] seeds)
        {
            visited = new HashSet<string>();
            robots = new Dictionary<string, Robots>();
            frontier = new Queue<Uri>();
            foreach (Uri seed in seeds)
            {
                frontier.Enqueue(seed);
            }
        }

        public IEnumerable<PageCrawl> Start()
        {
            while (frontier.Count > 0)
            {
                Console.WriteLine("Frontier size: " + frontier.Count);

                Uri nextCrawlLink = frontier.Dequeue();

                if (ShouldSkip(nextCrawlLink)) continue;

                latestCrawl = Browser.Crawl(new Page(nextCrawlLink));
                visited.Add(nextCrawlLink.ToString());
                // Ensure crawl didn't fail
                if (latestCrawl == null) continue;

                Uri[] newLinks = LinkParser.Parse(latestCrawl.Content, latestCrawl.Page.Uri);

                // TODO add use of database here
                // Add links and new crawl

                foreach (Uri uri in newLinks)
                {
                    if (!visited.Contains(uri.ToString()))
                    {
                        frontier.Enqueue(uri);
                    }
                }

                yield return latestCrawl;
                int millisecondDelay = robots[nextCrawlLink.AbsoluteUri].CrawlDelay * 1000;
                Console.WriteLine("Waiting for " + millisecondDelay + "ms");
                Thread.Sleep(millisecondDelay);
            }
        }

        /// <summary>
        /// Should the uri be skipped?
        /// Based on site's robots.txt and previously visited uris
        /// </summary>
        /// <param name="uri">The uri to check</param>
        /// <returns>True if the uri should be skipper</returns>
        private bool ShouldSkip(Uri uri)
        {
            // Skip visited
            if (visited.Contains(uri.ToString()))
            {
                Console.WriteLine("Already visited " + uri.ToString() + ", skipping");
                return true;
            }

            // Skip those blocked by robots
            if (!robots.ContainsKey(uri.AbsoluteUri))
            {
                robots.Add(uri.Authority, new Robots(uri, Browser.UserAgent));
            }
            Robots robot = robots[uri.AbsoluteUri];

            if (!robot.Allowed(uri))
            {
                Console.WriteLine("Robots.txt blocks access to " + uri.ToString() + ". Skipping." + Environment.NewLine);
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "Crawl Worker: " + Environment.NewLine
                + "Frontier size = " + frontier.Count + Environment.NewLine
                + "Latest Crawl = " + (latestCrawl != null ? latestCrawl.ToString() : "none");
        }
    }

}
