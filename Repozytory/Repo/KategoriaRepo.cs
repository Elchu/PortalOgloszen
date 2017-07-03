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