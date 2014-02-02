namespace Nelibur.ServiceModel.Contracts
{
    internal static class RestServiceMetadata
    {
        internal static class ParamName
        {
            public const string Data = "data";
            public const string Type = "type";
        }

        internal static class Path
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
