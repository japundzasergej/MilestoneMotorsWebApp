﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MilestoneMotorsWebApp.WebApi.Controllers
{
    public class BaseController(IMediator mediator) : Controller
    {
        protected readonly IMediator _mediator = mediator;
    }
}
