using System;
using Nelibur.ServiceModel.Contracts;
using Nelibur.ServiceModel.Extensions;
using Nelibur.ServiceModel.Services.Operations;
using Xunit;

namespace UnitTests.Nelibur.ServiceModel.Extensions
{
    public sealed class UrlExtensionsTest
    {
        private readonly Uri _serviceAddress = new Uri("http://nelibur.org/service");

        [Fact]
        public void ToUrl_DeleteWithResponse_Url()
        {
            var item = new Data { Id = 1 };
            string actual = item.ToUrl(_serviceAddress, OperationType.Delete);
            string expected = GetExpectedUrl(RestServiceMetadata.Path.Delete, item.QueryData());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToUrl_Delete_Url()
        {
            var item = new Data { Id = 1 };
            string actual = item.ToUrl(_serviceAddress, OperationType.Delete, false);
            string expected = GetExpectedUrl(RestServiceMetadata.Path.DeleteOneWay, item.QueryData());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToUrl_GetWithResponse_Url()
        {
            var item = new Data { Id = 1 };
            string actual = item.ToUrl(_serviceAddress, OperationType.Get);
            string expected = GetExpectedUrl(RestServiceMetadata.Path.Get, item.QueryData());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToUrl_Get_Url()
        {
            var item = new Data { Id = 1 };
            string actual = item.ToUrl(_serviceAddress, OperationType.Get, false);
            string expected = GetExpectedUrl(RestServiceMetadata.Path.GetOneWay, item.QueryData());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToUrl_PostWithResponse_Url()
        {
            var item = new Data { Id = 1 };
            string actual = item.ToUrl(_serviceAddress, OperationType.Post);
            string expected = GetExpectedUrl(RestServiceMetadata.Path.Post, item.QueryType());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToUrl_Post_Url()
        {
            var item = new Data { Id = 1 };
            string actual = item.ToUrl(_serviceAddress, OperationType.Post, false);
            string expected = GetExpectedUrl(RestServiceMetadata.Path.PostOneWay, item.QueryType());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToUrl_PutWithResponse_Url()
        {
            var item = new Data { Id = 1 };
            string actual = item.ToUrl(_serviceAddress, OperationType.Put);
            string expected = GetExpectedUrl(RestServiceMetadata.Path.Put, item.QueryType());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToUrl_Put_Url()
        {
            var item = new Data { Id = 1 };
            string actual = item.ToUrl(_serviceAddress, OperationType.Put, false);
            string expected = GetExpectedUrl(RestServiceMetadata.Path.PutOneWay, item.QueryType());
            Assert.Equal(expected, actual);
        }

        private string GetExpectedUrl(string path, string query)
        {
            return string.Format("{0}/{1}?{2}", _serviceAddress, path, query);
        }

        private sealed class Data
        {
            public int Id { get; set; }

            public string QueryData()
            {
                return string.Format("type={0}&Id={1}", GetType().Name, Id);
            }

            public string QueryType()
            {
                return string.Format("type={0}", GetType().Name);
            }
        }
    }
}
