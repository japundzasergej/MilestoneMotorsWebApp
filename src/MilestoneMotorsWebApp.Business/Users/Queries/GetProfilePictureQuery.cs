using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Users.Queries
{
    public class GetProfilePictureQuery : IRequest<ResponseDTO>
    {
        [FromRoute]
        public string Id { get; set; }
    }
}
