using System.Collections.Generic;
using System.Linq;
using Repozytory.Models;

namespace Repozytory.IRepo
{
    public interface IKategoriaRepo
    {
        IEnumerable<Kategoria> PobierzKategorie();
        Kategoria GetKategoriaById(int id);
        void SaveChanges();
    }
}