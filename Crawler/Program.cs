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
using System.IO;
using Crawler.Crawling.Interfaces;
using Crawler.Crashes;

namespace Crawler
{
    class Program
    {
        private static ICrawlDatabase database;

        static void Main(string[] args)
        {
            if (!HandleArgs(args)) return;

            CrawlWorker crawler = null;

            try
            {
                Uri[] seeds = new Uri[]
                   {
                new Uri("https://reddit.com/r/AskReddit/comments/c7yfub/what_did_a_crush_do_that_made_you_immediately/")
                   };
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

            foreach (PageCrawl crawl in crawler.Start())
            {
                database.InsertPageCrawl(crawl);
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

            database = new LoggerDatabase(args[0]);
            Console.WriteLine("Search: " + database.Name);
            Browser.UserAgentContactInformation = args[1];
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
