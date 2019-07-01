using Crawler.Internet;

namespace Crawler.Crawling.Interfaces
{
    internal interface ICrawlDatabase
    {
        string Name { get; }
        Link InsertLink(int sourcePageId, int destinationPageId, string linkText = null);
        PageCrawl InsertPageCrawl(PageCrawl crawl);
    }
}