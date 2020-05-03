using Crawler.Internet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler.Crawling
{
    /// <summary>
    /// Code duplicated from https://www.codeproject.com/script/Articles/ViewDownloads.aspx?aid=18050 "craigd"
    /// Distributed under the The Code Project Open License (CPOL) 1.02
    /// Modification by Sam Lord 2019
    /// </summary>
    class Robots
    {
        #region Private Fields: _FileContents, _UserAgent, _Server, _DenyUrls, _LogString
        private string _FileContents;
        private string _UserAgent;
        private string _Server;
        /// <summary>lowercase string array of url fragments that are 'denied' to the UserAgent for this RobotsTxt instance</summary>
        private ArrayList _DenyUrls = new ArrayList();
        private string _LogString = string.Empty;
        private int _crawlDelay = 10;

        /// <summary>
        /// A delay between 0 and 120 seconds
        /// (We don't mind being polite, but a potato can serve more than 1 page every 2 mins)
        /// </summary>
        public int CrawlDelay
        {
            get
            {
                return _crawlDelay < 121 ? _crawlDelay : 120;
            }
        }
        #endregion

        #region Constructors: require starting Url and UserAgent to create an object
        public Robots(Uri startPageUri, string userAgent, bool debug = false)
        {
            _UserAgent = userAgent;
            _Server = startPageUri.Host;
            
            try
            {
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://" + startPageUri.Authority + "/robots.txt");
                req.UserAgent = Browser.UserAgent;
                req.Accept = "text/plain";
                System.Net.HttpWebResponse webresponse = (System.Net.HttpWebResponse)req.GetResponse();

                using (System.IO.StreamReader stream = new System.IO.StreamReader(webresponse.GetResponseStream(), Encoding.ASCII))
                {
                    _FileContents = stream.ReadToEnd();
                }

                string[] fileLines = _FileContents.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if(fileLines.Length < 2)
                {
                    fileLines = _FileContents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                }

                bool rulesApply = false;
                foreach (string line in fileLines)
                {
                    RobotInstruction ri = new RobotInstruction(line);
                    if (ri.Instruction.Length < 1) continue;
                    switch (ri.Instruction.TrimStart()[0]) // trim as leading whitespace before comments is valid http://www.robotstxt.org/orig.html
                    {
                        case '#':   //then comment - ignore
                            break;
                        case 'u':   // User-Agent
                            if ((ri.UrlOrAgent == "*")
                              || (ri.UrlOrAgent.IndexOf(_UserAgent) >= 0))
                            { // these rules apply
                                rulesApply = true;
                                if (debug) Console.WriteLine(ri.UrlOrAgent + " " + rulesApply);
                            }
                            else
                            {
                                rulesApply = false;
                            }
                            break;
                        case 'd':   // Disallow
                            if (rulesApply)
                            {
                                _DenyUrls.Add(ri.UrlOrAgent.ToLower());
                                if (debug) Console.WriteLine("D " + ri.UrlOrAgent);
                            }
                            else
                            {
                                if (debug) Console.WriteLine("D " + ri.UrlOrAgent + " is for another user-agent");
                            }
                            break;
                        case 'a':   // Allow
                            if (debug) Console.WriteLine("A" + ri.UrlOrAgent);
                            break;
                        case 'c':
                            if (rulesApply)
                            {
                                if (debug) Console.WriteLine("C " + ri.UrlOrAgent);
                                _crawlDelay = Math.Abs(Convert.ToInt32(ri.UrlOrAgent));
                            }
                            break;
                        default:
                            // empty/unknown/error
                            Console.WriteLine("Unrecognised robots.txt entry [" + line + "]");
                            
                            break;
                    }
                }
            }
            catch (System.Net.WebException)
            {
                _FileContents = String.Empty;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Robots exception");
                Console.WriteLine("Will continue, but will be extra cautious as it could be our fault");
                Console.WriteLine("Attempted URL was " + "http://" + startPageUri.Authority + "/robots.txt");
                Console.WriteLine(ex.Message);
                _crawlDelay = 20;
                _FileContents = String.Empty;
            }
        }
        #endregion

        #region Methods: Allow
        /// <summary>
        /// Does the parsed robots.txt file allow this Uri to be spidered for this user-agent?
        /// </summary>
        /// <remarks>
        /// This method does all its "matching" in lowercase - it expects the _DenyUrl 
        /// elements to be ToLower() and it calls ToLower on the passed-in Uri...
        /// </remarks>
        public bool Allowed(Uri uri)
        {
            if (_DenyUrls.Count == 0) return true;

            string url = uri.AbsolutePath.ToLower();
            foreach (string denyUrlFragment in _DenyUrls)
            {
                if (url.Length >= denyUrlFragment.Length)
                {
                    if(ToRegex(denyUrlFragment).IsMatch(url))
                    {
                        return false;
                    } // else not a match
                } // else url is shorter than fragment, therefore cannot be a 'match'
            }
            if (url == "/robots.txt") return false;
            // no disallows were found, so allow
            return true;
        }

        /// <summary>
        /// Crude regex conversion of url fragment
        /// </summary>
        /// <param name="denyUrl">url fragment to convert</param>
        /// <returns></returns>
        private Regex ToRegex(string denyUrl)
        {
            try
            {
                string normalised = denyUrl
                            .Replace(".", @"\.") // escape periods
                            .Replace("/", @"\/") // escape forward slash
                            .Replace("*", ".+") // replace wildcard with regex wildcard
                            .Replace("(", @"\(") // open parentheses escape
                            .Replace(")", @"\)") // close parentheses escape
                            .Replace("?", @"\?") // escape question marks
                            + "(?:.?)+"; // include implicit trailing wildcard
                return new Regex(normalised, RegexOptions.IgnoreCase);
            }
            catch(Exception ex)
            {
                string text = "Robots failure to parse URL to normalised form" + Environment.NewLine
                    + ex.ToString() + Environment.NewLine
                    + "denyURL: " + denyUrl;

                Console.WriteLine(text);
                File.AppendAllText(@".\RobotsFailure_" + CrawlSettings.CrawlName + ".log", text);

                return new Regex(".+"); // skip failed parsings
            }
        }
        #endregion

        #region Private class: RobotInstruction
        /// <summary>
        /// Use this class to read/parse the robots.txt file
        /// </summary>
        /// <remarks>
        /// Types of data coming into this class
        /// User-agent: * ==> _Instruction='User-agent', _Url='*'
        /// Disallow: /cgi-bin/ ==> _Instruction='Disallow', _Url='/cgi-bin/'
        /// Disallow: /tmp/ ==> _Instruction='Disallow', _Url='/tmp/'
        /// Disallow: /~joe/ ==> _Instruction='Disallow', _Url='/~joe/'
        /// </remarks>
        private class RobotInstruction
        {
            private string _Instruction;
            private string _Url = string.Empty;

            /// <summary>
            /// Constructor requires a line, hopefully in the format [instuction]:[url]
            /// </summary>
            public RobotInstruction(string line)
            {
                string instructionLine = line;
                int commentPosition = instructionLine.IndexOf('#');
                if (commentPosition == 0)
                {
                    _Instruction = "#";
                }
                if (commentPosition >= 0)
                {   // comment somewhere on the line, trim it off
                    instructionLine = instructionLine.Substring(0, commentPosition);
                }
                if (instructionLine.Length > 0)
                {   // wasn't just a comment line (which should have been filtered out before this anyway
                    string[] lineArray = instructionLine.Split(':');
                    _Instruction = lineArray[0].Trim().ToLower();
                    if (lineArray.Length > 1)
                    {
                        _Url = lineArray[1].Trim();
                    }
                }
            }
            /// <summary>
            /// Lower-case part of robots.txt line, before the colon (:)
            /// </summary>
            public string Instruction
            {
                get { return _Instruction; }
            }

            /// <summary>
            /// Lower-case part of robots.txt line, after the colon (:)
            /// </summary>
            public string UrlOrAgent
            {
                get { return _Url; }
            }
        }
        #endregion
    }

}
