using Crawler.Internet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Crawling
{
    internal static class CrawlDatabase
    {
        private static string dbConnectionString = ConfigurationManager.ConnectionStrings["Crawler.Properties.Settings.CrawlsConnectionString"].ConnectionString;

        /// <summary>
        /// Add a crawl of a page to the database
        /// </summary>
        /// <param name="crawl">The crawl to add</param>
        public static void AddPageCrawl(PageCrawl crawl)
        {
            Host host = GetOrInsertHost(crawl);
            Page page = GetOrInsertPage(crawl, host);
            InsertCrawl(crawl, page.ID);
        }

        /// <summary>
        /// Gets the host of the crawl if present in the database or adds a new record
        /// </summary>
        /// <param name="crawl">The crawl whose host we wish to insert/retrieve</param>
        /// <returns>The host</returns>
        private static Host GetOrInsertHost(PageCrawl crawl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the page of the crawl if present in the database or adds a new record
        /// </summary>
        /// <param name="crawl"></param>
        private static Page GetOrInsertPage(PageCrawl crawl, Host host)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new crawl record to the database
        /// </summary>
        /// <param name="crawl">The page crawl to add</param>
        /// <param name="page">The associated page id</param>
        /// <returns>The PageCrawl with updated ID</returns>
        private static PageCrawl InsertCrawl(PageCrawl crawl, int pageId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add a link between two pages to the database
        /// </summary>
        /// <param name="sourcePageId">The page id of the source page</param>
        /// <param name="destinationPageId">The page id of the destination page</param>
        /// <param name="linkText">The readable text of the link</param>
        /// <returns></returns>
        public static Link InsertLink(int sourcePageId, int destinationPageId, string linkText = null)
        {
            throw new NotImplementedException();
        }
    }
}
