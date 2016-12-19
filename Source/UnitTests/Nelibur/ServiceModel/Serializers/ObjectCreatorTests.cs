using System;
using System.Collections.Specialized;
using System.Web;
using Nelibur.ServiceModel.Serializers;
using Xunit;

namespace UnitTests.Nelibur.ServiceModel.Serializers
{
    public class ObjectCreatorTests
    {
        [Fact]
        public void Create_QueryParamsCollectionContainsEscaping_Ok()
        {
            string query = string.Format("?value={0}", HttpUtility.UrlEncode("one + two"));
            ObjectCreator creator = new ObjectCreator(typeof(SampleRequest));
            NameValueCollection queryParams = HttpUtility.ParseQueryString(query);

            var result = creator.Create(queryParams);

            Assert.IsType<SampleRequest>(result);
            Assert.Equal("one + two", ((SampleRequest)result).Value);
        }


        private class SampleRequest
        {
            public string Value { get; set; }
        }
    }
}
