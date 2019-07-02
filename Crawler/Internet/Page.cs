using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Internet
{
    public class Page
    {
        private Uri uri;

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

        public Page(Uri uri)
        {
            this.uri = uri;
        }

        public override string ToString()
        {
            return Uri.ToString();
        }
    }
}
