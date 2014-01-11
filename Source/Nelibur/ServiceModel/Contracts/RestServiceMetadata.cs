namespace Nelibur.ServiceModel.Contracts
{
    internal static class RestServiceMetadata
    {
        internal static class Operations
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

        internal static class ParamNames
        {
            public const string Request = "request";
        }

        internal static class UriTemplates
        {
            public const string Delete = Operations.Delete + "/*";
            public const string DeleteWithResponse = Operations.DeleteWithResponse + "/*";
            public const string Get = Operations.Get + "/*";
            public const string GetWithResponse = Operations.GetWithResponse + "/*";
            public const string Post = Operations.Post;
            public const string PostWithResponse = Operations.PostWithResponse + "/";
            public const string Put = Operations.Put;
            public const string PutWithResponse = Operations.PutWithResponse + "/";
        }
    }
}
