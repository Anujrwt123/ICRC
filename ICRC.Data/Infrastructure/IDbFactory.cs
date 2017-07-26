using ICRC.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICRC.Data
{
    public interface IDbFactory:IDisposable
    {
        ICRCEntities Init();        
    }
}