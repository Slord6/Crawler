using Crawler.Crawling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Internet;

namespace Crawler.Crawling
{
    class LoggerDatabase : NamedDatabase
    {
        public LoggerDatabase()
        {
            this.name = CrawlSettings.CrawlName + "_loggerDb";
            Console.WriteLine("Logger database active - content will not be saved");
        }

        public override Link InsertLink(int sourcePageId, int destinationPageId, string linkText = null)
        {
            Console.WriteLine("LINK " + sourcePageId + ">" + destinationPageId + ". " + linkText);
            return null;
        }

        public override PageCrawl InsertPageCrawl(PageCrawl crawl)
        {
            Console.WriteLine("Mock database crawl insert:");
            Console.WriteLine(crawl.Page.Uri.ToString());
            Console.WriteLine(crawl.Content.Length + " bytes");
            Console.WriteLine(crawl.CrawlTime);
            Console.WriteLine();
            return crawl;
        }
    }
}
