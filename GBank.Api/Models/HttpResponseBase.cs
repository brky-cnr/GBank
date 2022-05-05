using System.Net;

namespace GBank.Api.Models
{
    public class HttpResponseBase
    {
        public ErrorModel Error { get; set; }
    }

    public class HttpResponseBase<TData>
    {
        public TData Data { get; set; }
        public HttpStatusCode Code { get; set; }
    }

    public class ErrorModel
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}