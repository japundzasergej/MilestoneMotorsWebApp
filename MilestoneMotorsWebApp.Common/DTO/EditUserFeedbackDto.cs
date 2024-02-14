using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneMotorsWebApp.Common.DTO
{
    public class EditUserFeedbackDto
    {
        public bool IsImageServiceDown { get; set; } = false;
        public bool HasFailed { get; set; } = false;
    }
}
