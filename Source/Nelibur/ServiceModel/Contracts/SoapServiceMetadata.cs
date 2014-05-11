using System;

namespace Nelibur.ServiceModel.Contracts
{
    public static class SoapServiceMetadata
    {
        public static class Action
        {
            public const string Process = "urn:nelibur-process";
            public const string ProcessOneWay = "urn:nelibur-processoneway";
            public const string ProcessResponse = "urn:nelibur-processresponse";
        }
    }
}
