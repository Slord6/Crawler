using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Internet;
using Crawler.Crawling;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Crawler
{
    class Program
    {

        static void Main(string[] args)
        {

            Uri[] seeds = new Uri[]
            {
                new Uri("https://reddit.com")
            };

            CrawlWorker crawler = new CrawlWorker(seeds);

            foreach (PageCrawl crawl in crawler.Start())
            {
                Console.WriteLine(crawl.CrawlTime);
                Console.WriteLine(crawl.Page.Uri.ToString());
                Console.WriteLine();                
            }

            Console.WriteLine("Ran out of links!");
            Console.ReadKey();
        }
    }
}
