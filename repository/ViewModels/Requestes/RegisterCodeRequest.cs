using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ViewModels.Requestes
{
    public class RegisterCodeRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
