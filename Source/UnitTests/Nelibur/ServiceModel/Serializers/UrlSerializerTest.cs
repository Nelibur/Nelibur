using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Serializers;
using Xunit;

namespace UnitTests.Nelibur.ServiceModel.Serializers
{
    public sealed class UrlSerializerTest
    {
        [Fact]
        public void FromQueryParams_NullQueryParams_ThrowException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => UrlSerializer.FromQueryParams(null));
        }

        [Fact]
        public void FromQueryParams_QueryParams_Ok()
        {
            const string TypeValue = "GetClientRequest";
            string query = string.Format("?type={0}&id=4DA2F891-953F-4637-AE83-6791688EAFA2&country=NL", TypeValue);
            NameValueCollection queryParams = HttpUtility.ParseQueryString(query);

            IUrlSerializer urlSerializer = UrlSerializer.FromQueryParams(queryParams);

            string actualType = urlSerializer.GetTypeValue();
            Assert.Equal(TypeValue, actualType);
            Assert.Equal(queryParams.Count, urlSerializer.QueryParams.Count);
        }

        [Fact]
        public void FromType_NullQueryParams_ThrowException()
        {
            Assert.Throws(typeof(ArgumentNullException), () => UrlSerializer.FromType(null));
        }

        [Fact]
        public void FromType_Type_Ok()
        {
            Type actualType = typeof(string);
            IUrlSerializer urlSerializer = UrlSerializer.FromType(actualType);
            Assert.Equal(1, urlSerializer.QueryParams.Count);
            Assert.Equal(actualType.Name, urlSerializer.GetTypeValue());
        }

        [Fact]
        public void FromValue_Value_Ok()
        {
            var request = new Request { Id = 1, Name = "Nelibur" };
            IUrlSerializer urlSerializer = UrlSerializer.FromValue(request);
            NameValueCollection actual = urlSerializer.QueryParams;
            Assert.Equal(3, urlSerializer.QueryParams.Count);
            var expected = new NameValueCollection
                           {
                               { RestServiceMetadata.ParamName.Type, "Request" },
                               { "Id", "1" },
                               { "Name", "Nelibur" }
                           };
            Assert.Equal(ToKeyValue(expected), ToKeyValue(actual));
        }

        private List<KeyValuePair<string, string>> ToKeyValue(NameValueCollection value)
        {
            var result = new List<KeyValuePair<string, string>>();
            foreach (string key in value.AllKeys)
            {
                result.Add(new KeyValuePair<string, string>(key, value[key]));
            }
            return result;
        }

        private sealed class Request
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
