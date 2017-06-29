using System.Linq;
using Repozytory.Models;

namespace Repozytory.IRepo
{
    public interface IOgloszenieRepo
    {
        IQueryable<Ogloszenie> PobierzOgloszenia();
    }
}