using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Internet
{
    public class Link
    {
        private Page sourcePage;
        private Page destinationPage;
        private string linkText;

        public Page Source
        {
            get
            {
                return sourcePage;
            }
        }

        public Page Destination
        {
            get
            {
                return destinationPage;
            }
        }

        public string Text
        {
            get
            {
                return linkText;
            }
        }

        public int ID { get; set; }

        public Link(Page sourcePage, Page destinationPage, string linkText = null)
        {
            this.sourcePage = sourcePage;
            this.destinationPage = destinationPage;
            this.linkText = linkText;
        }
    }
}
