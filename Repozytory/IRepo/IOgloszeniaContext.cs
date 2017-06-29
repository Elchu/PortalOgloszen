using System.Data.Entity;
using Repozytory.Models;

namespace Repozytory.IRepo
{
    public interface IOgloszeniaContext
    {
        DbSet<Kategoria> Kategorie { get; set; }
        DbSet<Ogloszenie> Ogloszenia { get; set; }
        DbSet<Uzytkownik> Uzytkownik { get; set; }
        DbSet<OgloszenieKategoria> OgloszenieKategoria { get; set; }

        int SaveChanges();
        Database Database { get; }
    }
}