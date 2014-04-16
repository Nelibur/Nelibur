using System;
using System.Collections.Specialized;

namespace Nelibur.ServiceModel.Serializers
{
    internal interface IUrlSerializer
    {
        NameValueCollection QueryParams { get; }
        object GetRequestValue(Type targetType);
        string GetTypeValue();
    }
}
