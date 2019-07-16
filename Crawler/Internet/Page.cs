using Crawler.Crawling;
using System;

namespace Crawler.Internet
{
    public class Page
    {
        private Uri uri;
        private Page linkedFrom;
        private PageCrawl crawl;

        public Uri Uri
        {
            get
            {
                return uri;
            }
        }

        public Page LinkedFrom
        {
            get
            {
                return linkedFrom;
            }
        }

        public PageCrawl Crawl
        {
            get
            {
                return crawl;
            }
            set
            {
                crawl = value;
            }
        }

        public int ID
        {
            get; set;
        }

        public Page(Uri uri, Page linkedFrom)
        {
            this.uri = uri;
            this.linkedFrom = linkedFrom;
            this.crawl = crawl;
        }

        public override string ToString()
        {
            return Uri.ToString();
        }
    }
}
