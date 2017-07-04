using System.Collections.Generic;
using System.Linq;
using Repozytory.IRepo;
using Repozytory.Models;

namespace Repozytory.Repo
{
    public class KategoriaRepo : IKategoriaRepo
    {
        private readonly IPortalOgloszenContext _db;

        public KategoriaRepo(IPortalOgloszenContext db)
        {
            _db = db;
        }

        public IEnumerable<Kategoria> PobierzKategorie()
        {
            return _db.Kategorie.ToList();
        }

        public IEnumerable<Ogloszenie> PobierzOgloszeniaZKategorii(int idKategorii)
        {
            var ogloszenia = (from o in _db.Ogloszenia
                              join k in _db.OgloszenieKategoria on o.OgloszenieId equals k.OgloszenieId
                              join u in _db.Uzytkownik on o.UzytkownikId equals u.Id
                              where k.KategoriaId == idKategorii
                              select o).ToList();


            return ogloszenia;
        }

        public Kategoria GetKategoriaById(int idKategorii)
        {
            return _db.Kategorie.Find(idKategorii);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}