using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MilestoneMotorsWebApp.Business.Accounts.Queries
{
    public class LogoutUserQuery : IRequest<bool> { }
}
