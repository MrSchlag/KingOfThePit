using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingOfThePit
{
    public interface IComponent
    {
        bool Update(float elapsed);
    }
}
