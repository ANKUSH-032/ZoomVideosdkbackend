using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zoom
{
    public class Zoom
    {
    }
    public class Response
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public string? Data { get; set; }
    }
    public class Response<T>
    {
        public string? Message { get; set; }
        public bool Status { get; set; }
        public T? Data { get; set; }
    }
    public class SessionAuthorizeResponse
    {
        public string Signature { get; set; } = string.Empty;
        public string SessionName { get; set; } = string.Empty;
        public string SessionPasscode { get; set; } = string.Empty;

    }
    public class ZoomConfig
    {
        public string SdkKey { get; set; } = string.Empty;
        public string SdkSecret { get; set; } = string.Empty;

    }
}
