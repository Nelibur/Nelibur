using System;
using System.Web;

namespace Nelibur.ServiceModel.Serializers
{
    public static class UrlEncoder
    {
        public static string Decode(string value)
        {
            return HttpUtility.UrlDecode(value);
        }

        /// <remarks>http://stackoverflow.com/questions/602642/server-urlencode-vs-httputility-urlencode</remarks>
        /// <remarks>http://blogs.msdn.com/b/yangxind/archive/2006/11/09/don-t-use-net-system-uri-unescapedatastring-in-url-decoding.aspx</remarks>
        public static string Encode(string value)
        {
            return Uri.EscapeDataString(value);
        }
    }
}
