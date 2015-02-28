using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Nelibur.ServiceModel.Contracts;
using Nelibur.Sword.Reflection;

namespace Nelibur.ServiceModel.Serializers
{
    internal sealed class ObjectCreator
    {
        private readonly ObjectActivator _objectActivator;
        private readonly Dictionary<string, PropertySetter> _setters;

        public ObjectCreator(Type type)
        {
            _objectActivator = DelegateFactory.CreateCtor(type);
            _setters = type.GetProperties()
                           .ToDictionary(x => x.Name, x => DelegateFactory.CreatePropertySetter(x), StringComparer.OrdinalIgnoreCase);
        }

        public object Create(NameValueCollection collection)
        {
            object result = _objectActivator();
            foreach (string key in collection.AllKeys)
            {
                if (string.Equals(key, RestServiceMetadata.ParamName.Type, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                string value = UrlEncoder.Decode(collection[key]);
                _setters[key](result, value);
            }
            return result;
        }
    }
}
