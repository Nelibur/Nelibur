using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Nelibur.ServiceModel.Contracts;
using Nelibur.Sword.Reflection;

namespace Nelibur.ServiceModel.Serializers
{
    internal sealed class QueryStringCreator
    {
        private readonly Dictionary<string, PropertyGetter> _getters;
        private readonly NameValueCollection _typeInfo;

        public QueryStringCreator(Type value)
        {
            _typeInfo = CreateQueryParams(value);
            _getters = value.GetProperties()
                            .ToDictionary(x => x.Name, x => DelegateFactory.CreatePropertyGetter(x));
        }

        public NameValueCollection Create(object value)
        {
            var result = new NameValueCollection(_typeInfo);
            foreach (KeyValuePair<string, PropertyGetter> item in _getters)
            {
                object rawValue = item.Value(value);
                if (rawValue == null)
                {
                    continue;
                }
                string itemValue = UrlEncoder.Encode(rawValue.ToString());
                result[item.Key] = itemValue;
            }
            return result;
        }

        private static NameValueCollection CreateQueryParams(Type value)
        {
            return new NameValueCollection
            {
                { RestServiceMetadata.ParamName.Type, UrlEncoder.Encode(value.Name) },
            };
        }
    }
}
