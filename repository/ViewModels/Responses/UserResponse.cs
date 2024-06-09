using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ViewModels.Responses
{
    public class UserResponse<T> : BaseResponse
    {
        public T Data { get; set; }
        public UserResponse(int error, string message, T data) : base(error, message)
        {
            Data = data;
        }
    }
}
