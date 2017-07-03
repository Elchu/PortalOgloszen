using System.Linq;
using Repozytory.Models;

namespace Repozytory.IRepo
{
    public interface IOgloszenieRepo
    {
        IQueryable<Ogloszenie> PobierzOgloszenia(string orderSort);
        Ogloszenie GetOgloszenieById(int id);
        void DodajOgloszenie(Ogloszenie ogloszenie);
        void UsunOgloszenie(int id);
        void Aktualizuj(Ogloszenie ogloszenie);
        IQueryable<Ogloszenie> PobierzOgloszeniaUzytkownikaPoId(string userId, string orderSort);
        IQueryable<Kategoria> PobierzKategorie();
        void DodajOgloszenieDoKategorii(int ogloszenieId, int kategoriaId);
        void SaveChanges();
    }
}