using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneMotorsWebApp.Business.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccessful { get; set; } = true;
        public object? Body { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<string>? ErrorList { get; set; } = null;
    }
}
