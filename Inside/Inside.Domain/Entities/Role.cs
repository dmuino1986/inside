using System;
using System.Collections.Generic;
using System.Text;
using Inside.Domain.Core;
using Microsoft.AspNetCore.Identity;

namespace Inside.Domain.Entities
{
   public class Role:IdentityRole<int>,IBaseEntity
   {
    }
}
