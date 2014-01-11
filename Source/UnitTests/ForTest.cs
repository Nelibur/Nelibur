using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Dispatcher;
using System.Text;
using Nelibur.Core.Extensions;
using Xunit;

namespace UnitTests
{
    public sealed class ForTest
    {
        public delegate object ObjectActivator();


        [Fact]
        public void Test()
        {
//            var dataRequest = new DataRequest { Id = 1 };
//            string url;
//            using (var stream = new MemoryStream())
//            {
//                var serializer = new DataContractJsonSerializer(dataRequest.GetType());
//                serializer.WriteObject(stream, dataRequest);
//                var converter = new QueryStringConverter();
//
//                url = converter.ConvertValueToString(stream.ToArray(), typeof(byte[]));
//            }
//
//            var queryCollection = new NameValueCollection { { "request", url } };
//            var uri = new Uri("http://localhost:9092/webhost");
//            UriBuilder t = new UriBuilder(uri)
//                .AddPath("sdfsdf")
//                .AddQuery(queryCollection);


                        Converter();
        }

        private static UriBuilder AddPath(UriBuilder builder, string pathValue)
        {
            string path = builder.Path;

            if (path.EndsWith("/") == false)
            {
                path = path + "/";
            }

            path += pathValue;
            builder.Path = path;
            return builder;
        }

        private static object CreateInstase(Type type)
        {
            ConstructorInfo emptyConstructor = GetEmptyConstructor(type);
            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);
            var t1 = (ObjectActivator)dynamicMethod.CreateDelegate(typeof(ObjectActivator));
            return t1();
        }

        private static ConstructorInfo GetEmptyConstructor(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        private static string Serialise(object value)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(value.GetType());
                serializer.WriteObject(stream, value);
                string content = Encoding.UTF8.GetString(stream.ToArray());
                return content;
            }
        }

        private void Converter()
        {
            var data = new List<DataRequest>
                {
                    new DataRequest(),
                    new DataRequest { Id = 1, CategoryName = "Test" },
                    new DataRequest { Id = 1 },
                };
            foreach (DataRequest dataRequest in data)
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(dataRequest.GetType());
                    serializer.WriteObject(stream, dataRequest);
                    var converter = new QueryStringConverter();

                    string url = converter.ConvertValueToString(stream.ToArray(), typeof(byte[]));
                    Console.WriteLine(url);
                    object rawObj = converter.ConvertStringToValue(url, typeof(byte[]));
                    stream.Position = 0;
                    object obj = serializer.ReadObject(new MemoryStream((byte[])rawObj));
                }
            }
        }

        private object SetValues(object instanse, string url)
        {
            var rawData = new Dictionary<string, string>();
            string[] values = url.Split('/');
            foreach (string rawPair in values)
            {
                string[] keyPair = rawPair.Split('=');
                if (keyPair.Length != 2)
                {
                    continue;
                }
                rawData[keyPair[0]] = keyPair[1];
            }
            PropertyInfo[] properties = instanse.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string value;
                if (rawData.TryGetValue(property.Name, out value))
                {
                    property.SetValue(instanse, (value));
                }
            }
            return instanse;
        }

        private string ToQueryString()
        {
            //            var converter = new QueryStringConverter();
            object dataRequest = new DataRequest();
            //            var t = converter.ConvertValueToString(dataRequest, typeof(object));

            //            var converter = new JsonQueryStringConverter();
            //           var t = converter.ConvertValueToString(dataRequest, typeof(object));

            var value = new DataRequest(); //{ CategoryName = "TestCategory", Id = 1};
            string url;
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(value.GetType());
                serializer.WriteObject(stream, value);
                //                url = HttpUtility.UrlEncode(stream.ToArray());

                var converter = new QueryStringConverter();
                url = converter.ConvertValueToString(stream.ToArray(), typeof(byte[]));

                object rawObj = converter.ConvertStringToValue(url, typeof(byte[]));

                stream.Position = 0;
                object obj = serializer.ReadObject(stream);
            }
            return url;


            //            var array = (from key in nvc.AllKeys
            //                         from value in nvc.GetValues(key)
            //                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
            //                .ToArray();
            //            return "?" + string.Join("&", array);
        }
    }

    public sealed class DataRequest
    {
        public string CategoryName { get; set; }
        public int Id { get; set; }
    }
}
