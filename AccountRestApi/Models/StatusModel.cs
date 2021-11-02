using System.Collections.Generic;

namespace AccountRestApi.Controllers
{
    public class StatusModel
    {
        public string Status { get; set; }
        public List<string> Errors { get; set; }
    }
}