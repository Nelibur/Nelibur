using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Web;
using Xunit;

namespace UnitTests
{
    public sealed class ForTest
    {

        public delegate object ObjectActivator();


        [Fact]
        public void Test()
        {
//            var t = new DataRequest{ CategoryName = "TestCategory", Id = 1};
//            var data = Serialise(t);
//
//            var t1 = new DataRequest{ CategoryName = "TestCategory"};
//            var data1 = Serialise(t1);
//
//            var t2 = new DataRequest{};
//            var data2 = Serialise(t2);

            Converter();

//            PropertyInfo[] properties = typeof(DataRequest).GetProperties();
//            var url = new StringBuilder();
//            foreach (var property in properties)
//            {
//                var value = property.GetValue(t).ToString();
//                Console.WriteLine(property.Name);
//                url.AppendFormat("{0}={1}/", property.Name, value);
//            }
//            Console.WriteLine(url.ToString());
//
//            var newInstanse = CreateInstase(typeof(DataRequest));
//
//            newInstanse = SetValues(newInstanse, url.ToString());

        }

//        public T GetFromQueryString<T>() where T : new()
//        {
//            var obj = new T();
//            var properties = typeof(T).GetProperties();
//            foreach (var property in properties)
//            {
//                var valueAsString = HttpContext.Current.Request.QueryString[property.PropertyName];
//                var value = Parse(valueAsString, property.PropertyType);
//
//                if (value == null)
//                    continue;
//
//                property.SetValue(obj, value, null);
//            }
//            return obj;
//        }

        private void Converter()
        {

            var data = new List<DataRequest>
            {
                new DataRequest(),
                new DataRequest{ Id = 1, CategoryName = "Test"},
                new DataRequest{ Id = 1},
            };
            foreach (var dataRequest in data)
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(dataRequest.GetType());
                    serializer.WriteObject(stream, dataRequest);
                    var converter = new QueryStringConverter();

                    string url = converter.ConvertValueToString(stream.ToArray(), typeof(byte[]));
                    Console.WriteLine(url);
                    var rawObj = converter.ConvertStringToValue(url, typeof(byte[]));
                    stream.Position = 0;
                    var obj = serializer.ReadObject(stream);
                }
            }
        }

        private string ToQueryString()
        {

//            var converter = new QueryStringConverter();
            object dataRequest = new DataRequest();
//            var t = converter.ConvertValueToString(dataRequest, typeof(object));

//            var converter = new JsonQueryStringConverter();
//           var t = converter.ConvertValueToString(dataRequest, typeof(object));

            var value = new DataRequest();//{ CategoryName = "TestCategory", Id = 1};
            string url;
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(value.GetType());
                serializer.WriteObject(stream, value);
//                url = HttpUtility.UrlEncode(stream.ToArray());

                var converter = new QueryStringConverter();
                url = converter.ConvertValueToString(stream.ToArray(), typeof(byte[]));

               var rawObj =  converter.ConvertStringToValue(url, typeof(byte[]));

               stream.Position = 0;
               var obj = serializer.ReadObject(stream);


            }
            return url;

            
//            var array = (from key in nvc.AllKeys
//                         from value in nvc.GetValues(key)
//                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
//                .ToArray();
//            return "?" + string.Join("&", array);
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

        private object SetValues(object instanse, string url)
        {
            var rawData = new Dictionary<string, string>();
            var values = url.Split('/');
            foreach (var rawPair in values)
            {
                var keyPair = rawPair.Split('=');
                if (keyPair.Length != 2)
                {
                    continue;
                }
                rawData[keyPair[0]] = keyPair[1];
            }
            PropertyInfo[] properties = instanse.GetType().GetProperties();
            foreach (var property in properties)
            {
                string value;
                if (rawData.TryGetValue(property.Name, out value))
                {
                    property.SetValue(instanse, ((object)value));
                }
            }
            return instanse;
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
    }

    public sealed class DataRequest
    {
        public string CategoryName { get; set; }
        public int Id { get; set; }
    }
}
