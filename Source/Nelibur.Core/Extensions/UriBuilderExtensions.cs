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
            builder.Path = string.Format(currentPath.EndsWith("/") ? "{0}{1}" : "{0}/{1}", currentPath, path);
            return builder;
        }

        public static UriBuilder AddQuery(this UriBuilder builder, NameValueCollection queryCollection)
        {
            string[] query = queryCollection
                .AllKeys.Select(key => string.Format("{0}={1}", key, queryCollection[key]))
                .ToArray();
            builder.Query = string.Join("&", query);
            return builder;
        }
    }
}
