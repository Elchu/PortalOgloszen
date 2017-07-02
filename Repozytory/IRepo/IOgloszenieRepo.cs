using System.Linq;
using Repozytory.Models;

namespace Repozytory.IRepo
{
    public interface IOgloszenieRepo
    {
        IQueryable<Ogloszenie> PobierzOgloszenia();
        Ogloszenie GetOgloszenieById(int id);
        void DodajOgloszenie(Ogloszenie ogloszenie);
        void UsunOgloszenie(int id);
        void Aktualizuj(Ogloszenie ogloszenie);
        IQueryable<Ogloszenie> PobierzStrone(int? page, int? pageSize);
        IQueryable<Kategoria> PobierzKategorie();
        void DodajOgloszenieDoKategorii(int ogloszenieId, int kategoriaId);
        void SaveChanges();
    }
}