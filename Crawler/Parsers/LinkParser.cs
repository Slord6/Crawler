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

                // This is a super naive way to deal with query params
                // TODO: improve
                linkValue = linkValue.Split('?')[0];
                try
                {
                    if (Uri.IsWellFormedUriString(linkValue, UriKind.Absolute))
                    {
                        links.Add(new Uri(linkValue));
                    }
                    else
                    {
                        links.Add(new Uri(source, linkValue));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERR: Invalid link, skipping. " + ex.Message);
                    Console.WriteLine(linkValue);
                }
            }
            return links.Distinct().ToArray();
        }
    }
}
