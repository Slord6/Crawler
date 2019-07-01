using Crawler.Crawling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler.Internet
{
    public static class Browser
    {
        private static string userAgentContactInfo = "";
        /// <summary>
        /// The user agent string of the crawler
        /// </summary>
        public static string UserAgent
        {
            get
            {
                return "CrawLord/" + Application.ProductVersion
                    + " (" + Environment.OSVersion + ") "
                    + UserAgentContactInformation;
            }
        }

        public static string UserAgentContactInformation
        {
            get
            {
                return userAgentContactInfo;
            }
            set
            {
                userAgentContactInfo = value;
            }
        }

        public static PageCrawl Crawl(Page page)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", Browser.UserAgent);
            try
            {
                string downloadString = client.DownloadString(page.Uri);

                return new PageCrawl(page, DateTime.Now, downloadString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERR - " + ex.Message);
                Console.WriteLine(page.Uri);
                Console.WriteLine();
                return null;
            }
        }
    }
}
