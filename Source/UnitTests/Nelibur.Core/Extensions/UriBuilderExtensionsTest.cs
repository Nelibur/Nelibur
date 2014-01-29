using System;
using Nelibur.Core.Extensions;
using Xunit;

namespace UnitTests.Nelibur.Core.Extensions
{
    public sealed class UriBuilderExtensionsTest
    {
        [Fact]
        public void AddPath_BaseAddressWithSlash_SlashNotAdded()
        {
            const string Url = "http://nelibur:9092/webhost/";
            var builder = new UriBuilder(new Uri(Url));
            const string Path = "Path";
            UriBuilder actual = builder.AddPath(Path);
            string expacted = string.Format("{0}{1}", Url, Path);
            Assert.Equal(expacted, actual.ToString());
        }

        [Fact]
        public void AddPath_BaseAddressWithoutSlash_SlashAdded()
        {
            const string Url = "http://nelibur:9092/webhost";
            var builder = new UriBuilder(new Uri(Url));
            const string Path = "Path";
            UriBuilder actual = builder.AddPath(Path);
            string expacted = string.Format("{0}/{1}", Url, Path);
            Assert.Equal(expacted, actual.ToString());
        }
    }
}
