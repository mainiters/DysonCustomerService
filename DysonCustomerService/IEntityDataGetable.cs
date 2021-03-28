using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonCustomerService
{
    public interface IEntityDataGetable
    {
        object GetIntityData(Guid EntityId);
    }
}
