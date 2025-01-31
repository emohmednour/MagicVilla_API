using System.Net;

namespace MagicVilla_VillaAPI.Models
{
    public class APIResponse
    {
        public APIResponse() {
            
            ErrorMessages = new List<string>(); 
        }
        public HttpStatusCode HttpStatusCode { get; set; }

        public List<string> ErrorMessages { get; set; }

        public bool IsSuccess { get; set; } = true;

        public object Result { get; set; }
    }
}
