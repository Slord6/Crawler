using Crawler.Internet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Crawling
{
    public class PageCrawl
    {
        private Page page;
        private DateTime crawlTime;
        private string content;

        public Page Page
        {
            get
            {
                return page;
            }
        }

        public DateTime CrawlTime
        {
            get
            {
                return crawlTime;
            }
        }

        public string Content
        {
            get
            {
                return content;
            }
        }

        public int ID { get; set; }

        public PageCrawl(Page page, DateTime crawlTime, string content)
        {
            this.page = page;
            this.crawlTime = crawlTime;
            this.content = content;
        }
    }
}
