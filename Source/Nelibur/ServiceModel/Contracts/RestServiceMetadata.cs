namespace Nelibur.ServiceModel.Contracts
{
    internal static class RestServiceMetadata
    {
        internal static class ParamName
        {
            public const string Request = "request";
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

        internal static class UriTemplate
        {
            public const string Delete = Path.Delete + "/*";
            public const string DeleteWithResponse = Path.DeleteWithResponse + "/*";
            public const string Get = Path.Get + "/*";
            public const string GetWithResponse = Path.GetWithResponse + "/*";
            public const string Post = Path.Post;
            public const string PostWithResponse = Path.PostWithResponse + "/";
            public const string Put = Path.Put;
            public const string PutWithResponse = Path.PutWithResponse + "/";
        }
    }
}
