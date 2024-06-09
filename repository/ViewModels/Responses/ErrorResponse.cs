using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ViewModels.Responses
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse(int error, string message) : base(error, message)
        {

        }
    }
}
