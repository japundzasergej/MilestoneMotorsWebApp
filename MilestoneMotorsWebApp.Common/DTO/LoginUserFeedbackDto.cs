using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneMotorsWebApp.Common.DTO
{
    public class LoginUserFeedbackDto
    {
        public bool IsNotPasswordsMatching { get; set; } = false;
        public bool IsValidUser { get; set; } = true;
    }
}
