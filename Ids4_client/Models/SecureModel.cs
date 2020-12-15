using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ids4_client.Models
{
    public class SecureModel
    {
        public string Token { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
