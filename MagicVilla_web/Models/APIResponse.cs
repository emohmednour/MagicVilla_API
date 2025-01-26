using System.Net;

namespace MagicVilla_web.Models
{
    public class APIResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public List<string> ErrorMessages { get; set; }

        public bool IsSuccess { get; set; }  = true;

        public object Result { get; set; }
    }
}
