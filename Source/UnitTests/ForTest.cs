using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.ServiceModel.Dispatcher;
using System.Web;
using Nelibur.ServiceModel.Serializers;
using Xunit;

namespace UnitTests
{
    public sealed class ForTest
    {
        private delegate object ObjectActivator();

        [Fact]
        public void Test()
        {
        }

        private static object CreateInstase(Type type)
        {
            ConstructorInfo emptyConstructor = GetEmptyConstructor(type);
            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);
            var activator = (ObjectActivator)dynamicMethod.CreateDelegate(typeof(ObjectActivator));
            return activator();
        }

        private static void CreateUrl()
        {
            string data = JsonDataSerializer.ToString(new Request());
            string url = HttpUtility.JavaScriptStringEncode(data);
            string url1 = HttpUtility.UrlEncode(data);
            string url2 = new JsonQueryStringConverter().ConvertValueToString(data, typeof(string));
            string url3 = Uri.EscapeDataString(data);
            Console.WriteLine(url);
            Console.WriteLine(url1);
            Console.WriteLine(url2);
            Console.WriteLine(url3);
            Console.WriteLine(HttpUtility.UrlDecode(url3));
            Console.WriteLine(data);
        }

        private static ConstructorInfo GetEmptyConstructor(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes);
        }

        [DataContract]
        private sealed class Request
        {
            public Request()
            {
                Id = Guid.NewGuid();
                Name = "Test";
            }

            [DataMember]
            public Guid Id { get; set; }

            [DataMember]
            public string Name { get; set; }
        }
    }
}
