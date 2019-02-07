using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace FaceDetection
{
    public class QueryBuilder
    {
        public StringBuilder sb { set; get; }

        public QueryBuilder()
        {
            sb = new StringBuilder();
            sb.Append("?");
        }

        public void Add(string key, string value)
        {
            sb.Append(key + "=" + value + "&");
        }

        public string Get()
        {
            sb.Length -= 1;
            return sb.ToString();
        }
    }
}
