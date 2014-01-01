namespace Nelibur.ServiceModel.Contracts
{
    internal static class ServiceMetadata
    {
        internal static class Operations
        {
            public const string Process = "urn:nelibur-process";
            public const string ProcessResponse = Process + "response";
            public const string ProcessWithoutResponse = "urn:nelibur-processwithoutresponse";
        }
    }
}
