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

        public IEnumerable<Ogloszenie> PobierzOgloszeniaZKategorii(int id)
        {
            var ogloszenia = (from o in _db.Ogloszenia
                join k in _db.OgloszenieKategoria on o.OgloszenieId equals k.OgloszenieId
                join u in _db.Uzytkownik on o.UzytkownikId equals u.Id
                where k.KategoriaId == id
                select o).ToList();
            

            return ogloszenia;
        }

        public Kategoria GetKategoriaById(int id)
        {
            return _db.Kategorie.Find(id);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}