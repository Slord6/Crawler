using System;

namespace Crawler.Internet
{
    public class Page
    {
        private Uri uri;
        private Page linkedFrom;

        public Uri Uri
        {
            get
            {
                return uri;
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
        }

        public override string ToString()
        {
            return Uri.ToString();
        }
    }
}
