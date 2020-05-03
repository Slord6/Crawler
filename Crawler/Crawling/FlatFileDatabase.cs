using Crawler.Crawling.Interfaces;
using Crawler.Internet;
using System;
using System.IO;

namespace Crawler.Crawling
{
    internal class FlatFileDatabase : ICrawlDatabase
    {
        private static readonly string DATA_SEPARATOR = Environment.NewLine + "==-DATA__SEP-==" + Environment.NewLine;
        private string name;

        public string Name => name;

        public FlatFileDatabase(string name)
        {
            this.name = name;
        }

        public Link InsertLink(int sourcePageId, int destinationPageId, string linkText = null)
        {
            throw new NotImplementedException();
        }

        public PageCrawl InsertPageCrawl(PageCrawl crawl)
        {
            Console.WriteLine("FILE DB");
            string crawlPath = GetPageCrawlPath(crawl);

            // Ensure contains no invalid chars
            foreach (char invalidChar in Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()))
            {
                crawlPath.Replace(invalidChar, '_');
            }

            // creating an existing directory has no effect so we can do that every time
            Directory.CreateDirectory(Path.GetDirectoryName(crawlPath));
            StreamWriter streamWriter = File.CreateText(crawlPath);

            streamWriter.Write(crawl.Page.Uri.ToString());
            streamWriter.Write(DATA_SEPARATOR);
            streamWriter.Write(crawl.Page.LinkedFrom);
            streamWriter.Write(DATA_SEPARATOR);
            streamWriter.Write(crawl.CrawlTime);
            streamWriter.Write(DATA_SEPARATOR);
            streamWriter.Write(crawl.Content);
            streamWriter.Close();

            Console.WriteLine("Saving crawl to " + crawlPath);

            return crawl;
        }

        protected string GetDatabaseDir()
        {
            return "./" + this.name;
        }

        protected string GetHostPath(PageCrawl crawl)
        {
            return GetDatabaseDir() + "/" + crawl.Page.Uri.Host;
        }

        protected string GetPageCrawlPath(PageCrawl crawl)
        {
            return GetHostPath(crawl) + crawl.Page.Uri.AbsolutePath + "-" + crawl.CrawlTime.ToString("yyyy'-'MM'-'dd-HH'-'mm'-'ss") + ".txt";
        }
    }
}
