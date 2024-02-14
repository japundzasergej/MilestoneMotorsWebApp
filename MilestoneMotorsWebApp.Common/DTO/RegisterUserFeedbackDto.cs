using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MilestoneMotorsWebApp.Common.DTO
{
    public class RegisterUserFeedbackDto
    {
        public bool UserExists { get; set; } = false;
        public bool ResponseFailed { get; set; } = false;
        public List<IdentityError> ErrorList { get; set; } = [ ];
    }
}
