using Crawler.Crawling.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Internet;

namespace Crawler.Crawling
{
    abstract class NamedDatabase : ICrawlDatabase
    {
        protected string name;

        string ICrawlDatabase.Name
        {
            get
            {
                return name;
            }
        }

        public virtual Link InsertLink(int sourcePageId, int destinationPageId, string linkText = null)
        {
            throw new NotImplementedException();
        }

        public virtual PageCrawl InsertPageCrawl(PageCrawl crawl)
        {
            throw new NotImplementedException();
        }
    }
}
