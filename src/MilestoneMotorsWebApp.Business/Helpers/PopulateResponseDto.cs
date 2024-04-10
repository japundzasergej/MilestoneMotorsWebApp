using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilestoneMotorsWebApp.Business.DTO;

namespace MilestoneMotorsWebApp.Business.Helpers
{
    public static class PopulateResponseDto
    {
        public static ResponseDTO OnSuccess<TBody>(TBody body, int statusCode)
        {
            return new ResponseDTO { StatusCode = statusCode, Body = body, };
        }

        public static ResponseDTO OnFailure(int statusCode, object? body = null)
        {
            return new ResponseDTO()
            {
                StatusCode = statusCode,
                Body = body,
                IsSuccessful = false
            };
        }

        public static ResponseDTO OnError(Exception e, int statusCode = 500)
        {
            return new ResponseDTO
            {
                IsSuccessful = false,
                StatusCode = statusCode,
                ErrorList =  [ e.Message ]
            };
        }
    }
}
