using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
        DbEntityEntry Entry(object entity);
        Database Database { get; }
    }
}