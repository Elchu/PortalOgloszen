using System.Linq;
using Repozytory.Models;

namespace Repozytory.IRepo
{
    public interface IKategoriaRepo
    {
        IQueryable<Kategoria> PobierzKategorie();
        Kategoria GetKategoriaById(int id);
        void SaveChanges();
    }
}