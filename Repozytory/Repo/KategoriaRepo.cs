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

        public IQueryable<Kategoria> PobierzKategorie()
        {
            return _db.Kategorie;
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