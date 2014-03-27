using System;

namespace Nelibur.ServiceModel.Contracts
{
    public static class RestServiceMetadata
    {
        public static class ParamName
        {
            public const string Data = "data";
            public const string Type = "type";
        }

        public static class Path
        {
            public const string Delete = "Delete";
            public const string DeleteWithResponse = "DeleteWithResponse";
            public const string Get = "Get";
            public const string GetWithResponse = "GetWithResponse";
            public const string Post = "Post";
            public const string PostWithResponse = "PostWithResponse";
            public const string Put = "Put";
            public const string PutWithResponse = "PutWithResponse";
        }
    }
}
