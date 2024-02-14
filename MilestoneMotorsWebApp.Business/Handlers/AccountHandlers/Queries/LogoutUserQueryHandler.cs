using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MilestoneMotorsWebApp.Business.Accounts.Queries;
using MilestoneMotorsWebApp.Domain.Entities;

namespace MilestoneMotorsWebApp.Business.Handlers.AccountHandlers.Queries
{
    public class LogoutUserQueryHandler(SignInManager<User> signInManager)
        : IRequestHandler<LogoutUserQuery, bool>
    {
        private readonly SignInManager<User> _signInManager = signInManager;

        public async Task<bool> Handle(LogoutUserQuery request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
