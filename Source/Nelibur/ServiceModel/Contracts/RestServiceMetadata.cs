using System;

namespace Nelibur.ServiceModel.Contracts
{
    public static class RestServiceMetadata
    {
        public static class ParamName
        {
            public const string Type = "type";
        }

        public static class Path
        {
            public const string Delete = "Delete";
            public const string DeleteOneWay = "DeleteOneWay";
            public const string Get = "Get";
            public const string GetOneWay = "GetOneWay";
            public const string Post = "Post";
            public const string PostOneWay = "PostOneWay";
            public const string Put = "Put";
            public const string PutOneWay = "PutOneWay";
        }
    }
}
