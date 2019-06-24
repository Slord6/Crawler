using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Internet
{
    class Host
    {
        private int id;
        private Uri uri;

        public string Name
        {
            get
            {
                return uri.Host;
            }
        }

        public int ID
        {
            get
            {
                return id;
            }
        }

        public Host(Uri uri)
        {

        }

        public Host(int id, Uri uri)
        {

        }
    }
}
