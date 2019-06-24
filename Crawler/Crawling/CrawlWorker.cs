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
        private int waitTime;
        private Queue<Uri> frontier;
        private HashSet<string> visited;

        public CrawlWorker(Uri[] seeds)
        {
            visited = new HashSet<string>();
            frontier = new Queue<Uri>();
            foreach(Uri seed in seeds)
            {
                frontier.Enqueue(seed);
            }
            waitTime = 5000;
        }

        public IEnumerable<PageCrawl> Start()
        {
            while (frontier.Count > 0)
            {
                Console.WriteLine("Frontier size: " + frontier.Count);

                Uri nextCrawlLink = frontier.Dequeue();

                // Skip visited
                if (visited.Contains(nextCrawlLink.ToString())) continue;
                visited.Add(nextCrawlLink.ToString());

                PageCrawl crawl = Browser.Crawl(new Page(nextCrawlLink));
                // Ensure crawl didn't fail
                if (crawl == null) continue;

                Uri[] newLinks = LinkParser.Parse(crawl.Content, crawl.Page.Uri);

                // TODO add use of database here
                // Add links and new crawl
                
                foreach(Uri uri in newLinks)
                {
                    if(!visited.Contains(uri.ToString())) {
                        frontier.Enqueue(uri);
                    }
                }

                yield return crawl;
                Thread.Sleep(waitTime);
            }
        }
    }
}
