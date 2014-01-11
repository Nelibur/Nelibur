using System;
using System.Collections.Specialized;
using System.Linq;

namespace Nelibur.Core.Extensions
{
    public static class UriBuilderExtensions
    {
        public static UriBuilder AddPath(this UriBuilder builder, string path)
        {
            string currentPath = builder.Path;
            if (currentPath.EndsWith("/") == false)
            {
                currentPath = currentPath + "/";
            }

            currentPath += path + "/";
            builder.Path = currentPath;
            return builder;
        }

        public static UriBuilder AddQuery(this UriBuilder builder, NameValueCollection queryCollection)
        {
            string[] query = (from key in queryCollection.AllKeys
                from value in queryCollection.GetValues(key)
                select string.Format("{0}={1}", key, value))
                .ToArray();
            builder.Query = string.Join("&", query);
            return builder;
        }
    }
}
