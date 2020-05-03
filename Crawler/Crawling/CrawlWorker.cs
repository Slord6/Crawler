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
        private Queue<Page> frontier;
        private HashSet<string> visited;
        private Dictionary<string, Robots> robots;
        private PageCrawl latestCrawl;

        public string Frontiers
        {
            get
            {
                return string.Join(Environment.NewLine, frontier);
            }
        }

        public CrawlWorker(Uri[] seeds)
        {
            visited = new HashSet<string>();
            robots = new Dictionary<string, Robots>();
            frontier = new Queue<Page>();
            foreach (Uri seed in seeds)
            {
                frontier.Enqueue(new Page(seed, null));
            }
        }

        public IEnumerable<PageCrawl> Start()
        {
            while (frontier.Count > 0)
            {
                Console.WriteLine("Frontier size: " + frontier.Count);

                Page currentCrawlPage = frontier.Dequeue();

                if (ShouldSkip(currentCrawlPage.Uri)) continue;

                latestCrawl = Browser.Crawl(currentCrawlPage);
                
                currentCrawlPage.Crawl = latestCrawl;
                visited.Add(currentCrawlPage.ToString());
                // Ensure crawl didn't fail
                if (latestCrawl == null) continue;

                Uri[] newLinks = LinkParser.Parse(latestCrawl.Content, latestCrawl.Page.Uri);
                
                // Add links and new crawl

                foreach (Uri uri in newLinks)
                {
                    if (!visited.Contains(uri.ToString()))
                    {
                        frontier.Enqueue(new Page(uri, currentCrawlPage));
                    }
                }

                yield return latestCrawl;
                int millisecondDelay = robots[currentCrawlPage.Uri.Authority].CrawlDelay * 1000;
                millisecondDelay = (int)Math.Max(0, millisecondDelay - (DateTime.Now - latestCrawl.CrawlTime).TotalMilliseconds);
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
            if (!robots.ContainsKey(uri.Authority))
            {
                // we just use the first part to determine if we're blocked - CrawLord/[version]
                robots.Add(uri.Authority, new Robots(uri, Browser.UserAgent.Split(' ')[0]));
            }
            Robots robot = robots[uri.Authority];

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
                + "Latest Crawl = " + (latestCrawl != null ? latestCrawl.ToString() : "none")
                + Environment.NewLine + Environment.NewLine
                + "Frontier size = " + frontier.Count + Environment.NewLine
                + "Frontier = " + Environment.NewLine + Frontiers;
        }
    }

}
