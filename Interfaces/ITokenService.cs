using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PRUEBA_TECNICA_EY.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user);
    }
}