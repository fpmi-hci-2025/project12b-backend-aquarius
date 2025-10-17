using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Request;

public class LoginRequest
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
