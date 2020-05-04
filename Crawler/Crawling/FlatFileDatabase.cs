using Crawler.Crawling.Interfaces;
using Crawler.Internet;
using System;
using System.IO;
using System.Linq;

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
                if (invalidChar == '/' || invalidChar == '\\') continue; // leaves slashes to preserve file path

                crawlPath = crawlPath.Replace(invalidChar, '_');
            }

            if(String.IsNullOrWhiteSpace(crawlPath) || String.IsNullOrEmpty(crawlPath))
            {
                crawlPath = "%20";
            }

            // creating an existing directory has no effect so we can do that every time
            try
            {
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
            }
            catch(Exception ex)
            {
                string errMsg = "Failed to save crawl to disk - " + crawl.Page.Uri.ToString() + " at " + crawlPath;
                Console.WriteLine(errMsg);
                throw new IOException(errMsg, ex);
            }

            Console.WriteLine("Saved crawl to " + crawlPath);

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
