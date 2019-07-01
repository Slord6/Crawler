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
            if (!HandleArgs(args)) return;

            Uri[] seeds = new Uri[]
            {
                new Uri("https://reddit.com")
            };

            CrawlWorker crawler = new CrawlWorker(seeds);

            foreach (PageCrawl crawl in crawler.Start())
            {
                Console.WriteLine(crawl.CrawlTime);
                Console.WriteLine(crawl.Page.Uri.ToString());
                Console.WriteLine(crawl.Content.Length + " bytes");
                Console.WriteLine();
            }

            Console.WriteLine("Ran out of links!");
            Console.ReadKey();
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
            Browser.UserAgentContactInformation = args[1];
            Console.WriteLine("Search: " + args[0]);
            Console.WriteLine("User Agent: " + Browser.UserAgent);
            Console.WriteLine();
            return true;
        }

        private static void PrintHelp()
        {
            Console.WriteLine();
            Console.WriteLine(" =: CRAWLER HELP :=");
            Console.WriteLine("Argument 1:");
            Console.WriteLine("\t(Not currently used) Unique name for this search, used for storage/resume of crawls");
            Console.WriteLine("Argument 2:");
            Console.WriteLine("\tContact details, in case a bug causes problems for others");
            Console.WriteLine();
        }
    }
}
