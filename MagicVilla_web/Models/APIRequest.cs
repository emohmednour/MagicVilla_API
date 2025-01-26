using System.Security.AccessControl;
using static MagicVilla_Utility.SD;

namespace MagicVilla_web.Models
{
    public class APIRequest
    {
        public APItype ApiType { get; set; } = APItype.GET;
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
