using Crawler.Crawling.Interfaces;
using Crawler.Internet;
using System;

namespace Crawler.Crawling
{
    internal class FlatFileDatabase : ICrawlDatabase
    {
        private string crawlName;

        public string Name
        {
            get
            {
                return crawlName;
            }
        }

        public FlatFileDatabase(string name)
        {
            this.crawlName = name;
        }

        /// <summary>
        /// Add a crawl of a page to the database
        /// </summary>
        /// <param name="crawl">The crawl to add</param>
        public PageCrawl InsertPageCrawl(PageCrawl crawl)
        {
            Host host = GetOrInsertHost(crawl);
            Page page = GetOrInsertPage(crawl, host);
            return InsertCrawl(crawl, page.ID);
        }

        /// <summary>
        /// Add a link between two pages to the database
        /// </summary>
        /// <param name="sourcePageId">The page id of the source page</param>
        /// <param name="destinationPageId">The page id of the destination page</param>
        /// <param name="linkText">The readable text of the link</param>
        /// <returns></returns>
        public Link InsertLink(int sourcePageId, int destinationPageId, string linkText = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the host of the crawl if present in the database or adds a new record
        /// </summary>
        /// <param name="crawl">The crawl whose host we wish to insert/retrieve</param>
        /// <returns>The host</returns>
        private Host GetOrInsertHost(PageCrawl crawl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the page of the crawl if present in the database or adds a new record
        /// </summary>
        /// <param name="crawl"></param>
        private Page GetOrInsertPage(PageCrawl crawl, Host host)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new crawl record to the database
        /// </summary>
        /// <param name="crawl">The page crawl to add</param>
        /// <param name="pageId">The associated page id</param>
        /// <returns>The PageCrawl with updated ID</returns>
        private PageCrawl InsertCrawl(PageCrawl crawl, int pageId)
        {
            throw new NotImplementedException();
        }
    }
}
