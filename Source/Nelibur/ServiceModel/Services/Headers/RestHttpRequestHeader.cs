namespace Nelibur.ServiceModel.Services.Headers
{
    internal abstract class RestHttpRequestHeader
    {
        public abstract string Name { get; }
        public abstract string Value { get; protected set; }
    }
}
