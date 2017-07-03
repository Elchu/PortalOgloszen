using System.Collections.Generic;
using System.Linq;
using Repozytory.Models;

namespace Repozytory.IRepo
{
    public interface IOgloszenieRepo
    {
        IEnumerable<Ogloszenie> PobierzOgloszenia(string orderSort);
        Ogloszenie GetOgloszenieById(int id);
        void DodajOgloszenie(Ogloszenie ogloszenie);
        void UsunOgloszenie(int id);
        void Aktualizuj(Ogloszenie ogloszenie);
        IEnumerable<Ogloszenie> PobierzOgloszeniaUzytkownikaPoId(string userId, string orderSort);
        IEnumerable<Kategoria> PobierzKategorie();
        void DodajOgloszenieDoKategorii(int ogloszenieId, int kategoriaId);
        void SaveChanges();
    }
}