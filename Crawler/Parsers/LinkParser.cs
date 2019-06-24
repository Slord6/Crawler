using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler.Parsers
{
    public static class LinkParser
    {
        /// <summary>
        /// Extract links from a string
        /// </summary>
        /// <returns>Array of found links</returns>
        public static Uri[] Parse(string content, Uri source)
        {
            Regex regex = new Regex("<a.*?href=(?:\"|')((?:.+?))(?:\"|').*?>");
            MatchCollection matches = regex.Matches(content);
            List<Uri> links = new List<Uri>();

            foreach(Match match in matches)
            {
                string linkValue = match.Groups[1].Value;
                
                if (Uri.IsWellFormedUriString(linkValue, UriKind.Absolute))
                {
                    links.Add(new Uri(linkValue));
                }
                else
                {
                    links.Add(new Uri(source, linkValue));
                }
            }
            return links.Distinct().ToArray();
        }
    }
}
