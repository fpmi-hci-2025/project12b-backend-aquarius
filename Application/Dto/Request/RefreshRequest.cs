using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Request;

public class RefreshRequest
{
    public string RefreshToken { get; set; }
    public string AccessToken { get; set; }
}
